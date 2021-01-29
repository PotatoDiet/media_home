package controllers

import (
  "potatodiet/media_home_backend/items"
  "potatodiet/media_home_backend/providers"
  "github.com/labstack/echo/v4"
  "path/filepath"
  "net/http"
  "os"
  "strconv"
  "fmt"
)

func (c Controller) Video(ctx echo.Context) error {
  var video items.Video
  c.DB.First(&video, ctx.Param("id"))
  return ctx.JSON(http.StatusOK, video)
}

func (c Controller) VideoUpdateWatchTimestamp(ctx echo.Context) error {
  var video items.Video
  c.DB.First(&video, ctx.Param("id"))

  timestamp, err := strconv.ParseUint(ctx.QueryParam("timestamp"), 10, 64)
  if err != nil {
    return err
  }
  video.CurrentWatchTimestamp = timestamp
  c.DB.Save(&video)

  return ctx.NoContent(http.StatusOK)
}

func (c Controller) Videos(ctx echo.Context) error {
  var videos []items.Video

  search := ctx.QueryParam("search")
  fmt.Println(search);
  if search == "" {
    c.DB.Order("title").Find(&videos)
  } else {
    c.DB.Where("title like ?", "%" + search + "%").Find(&videos)
  }

  return ctx.JSON(http.StatusOK, videos)
}

func (c Controller) VideosUpdate(ctx echo.Context) error {
  providers_list := []providers.Provider{
    providers.TmdbInit("5ef572428a688eddbb5e68049f7fedd8"),
  }

  for _, path := range find_videos() {
    var v items.Video
    c.DB.FirstOrCreate(&v, items.Video{Location: path})

    for _, provider := range providers_list {
      provider.Find(&v)
    }

    c.DB.Save(&v)
  }

  return ctx.NoContent(http.StatusOK)
}

func (c Controller) VideosClean(ctx echo.Context) error {
  var videos []items.Video
  c.DB.Find(&videos)
  for _, v := range videos {
    if _, err := os.Stat(v.Location); os.IsNotExist(err) {
      c.DB.Delete(&v)
    }
  }

  return ctx.NoContent(http.StatusOK)
}

func find_videos() []string {
  var videos []string

  filepath.Walk("videos", func(path string, _ os.FileInfo, _ error) error {
    extensions := map[string]bool {
      ".mkv": true,
      ".mp4": true,
    }

    ext := filepath.Ext(path)
    if extensions[ext] {
      videos = append(videos, path)
    }

    return nil
  })

  return videos
}
