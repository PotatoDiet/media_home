package controllers

import (
	"os"
	"os/exec"
	"path/filepath"
	"potatodiet/media_home_backend/items"

	"github.com/labstack/echo/v4"
)

// Stream returns a video file, specified by id.
func (c Controller) Stream(ctx echo.Context) error {
	var video items.Video
	c.DB.First(&video, ctx.Param("id"))

	ext := filepath.Ext(video.Location)
	if ext == ".mp4" {
		return ctx.File(video.Location)
	}

	out := "transcodes/" + video.Location + ".mp4"
	os.MkdirAll(filepath.Dir(out), os.ModePerm)

	cmd := exec.Command(
		"ffmpeg",
		"-i", video.Location,
		"-codec", "copy",
		out,
	)
	cmd.Start()
	cmd.Wait()

	return ctx.File(out)
}
