package main

import (
	"net/http"
	"potatodiet/media_home/controllers"

	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
	"gorm.io/driver/sqlite"
	"gorm.io/gorm"
)

func main() {
	e := echo.New()
	e.Static("/assets", "assets")

	e.Use(middleware.CORSWithConfig(middleware.CORSConfig{
		AllowOrigins: []string{"http://localhost:3000"},
		AllowMethods: []string{http.MethodGet, http.MethodPut, http.MethodPost, http.MethodDelete},
	}))

	db, err := gorm.Open(sqlite.Open("dev.db"), &gorm.Config{})
	if err != nil {
		panic("error connecting to db")
	}

	controller := controllers.Controller{
		DB: db,
	}

	e.GET("/video/:id", controller.Video)
	e.GET("/video/:id/update_watch_timestamp", controller.VideoUpdateWatchTimestamp)
	e.GET("/videos", controller.Videos)
	e.GET("/videos/update", controller.VideosUpdate)
	e.GET("/videos/clean", controller.VideosClean)
	e.GET("/stream/:id", controller.Stream)

	e.Start(":1234")
}
