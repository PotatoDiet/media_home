package controllers

import (
  "potatodiet/media_home_backend/items"
  "potatodiet/media_home_backend/providers"
  "github.com/labstack/echo/v4"
  "path/filepath"
  "net/http"
  "os"
)

func (c Controller) Video(ctx echo.Context) error {
  var video items.Video
  c.DB.First(&video, ctx.Param("id"))
  return ctx.JSON(http.StatusOK, video)
}

func (c Controller) Videos(ctx echo.Context) error {
  var videos []items.Video
  c.DB.Order("title").Find(&videos)
  return ctx.JSON(http.StatusOK, videos)
}

func (c Controller) VideosUpdate(ctx echo.Context) error {
  providers_list := []providers.Provider{
    *providers.OmdbInit("640e7773"),
  }

  videos, _ := filepath.Glob("videos/*.mkv")
  for _, path := range videos {
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