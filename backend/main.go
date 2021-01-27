package main

import (
  "potatodiet/media_home_backend/controllers"
  "potatodiet/media_home_backend/items"
  "net/http"
  "github.com/labstack/echo/v4"
  "github.com/labstack/echo/v4/middleware"
  "gorm.io/gorm"
  "gorm.io/driver/sqlite"
)

type Library struct {
  ID uint `json:"id"`
  Name string `json:"name"`
  Location string `json:"location"`
}

func main() {
  db, err := gorm.Open(sqlite.Open("test.db"), &gorm.Config{})
  if err != nil {
    panic("error connecting to db")
  }

  db.AutoMigrate(&items.Video{})

  e := echo.New()
  e.Static("/assets", "assets")

  e.Use(middleware.CORSWithConfig(middleware.CORSConfig{
    AllowOrigins: []string{"http://localhost:3000"},
		AllowMethods: []string{http.MethodGet, http.MethodPut, http.MethodPost, http.MethodDelete},
	}))

  c := controllers.Controller{
    DB: db,
  }

  e.GET("/video/:id", c.Video)
  e.GET("/video/:id/update_watch_timestamp", c.VideoUpdateWatchTimestamp)
  e.GET("/videos", c.Videos)
  e.GET("/videos/update", c.VideosUpdate)
  e.GET("/videos/clean", c.VideosClean)
  e.GET("/stream/:id", c.Stream)

  e.Start(":1234")
}
