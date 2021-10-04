package main

import (
	"net/http"
	"potatodiet/media_home_backend/controllers"
	"potatodiet/media_home_backend/items"

	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
	"gorm.io/driver/sqlite"
	"gorm.io/gorm"
)

type library struct {
	ID       uint   `json:"id"`
	Name     string `json:"name"`
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

	e.Use(dbInjector(db))

	e.Use(middleware.CORSWithConfig(middleware.CORSConfig{
		AllowOrigins: []string{"http://localhost:3000"},
		AllowMethods: []string{http.MethodGet, http.MethodPut, http.MethodPost, http.MethodDelete},
	}))

	e.GET("/video/:id", controllers.Video)
	e.GET("/video/:id/update_watch_timestamp", controllers.VideoUpdateWatchTimestamp)
	e.GET("/videos", controllers.Videos)
	e.GET("/videos/update", controllers.VideosUpdate)
	e.GET("/videos/clean", controllers.VideosClean)
	e.GET("/stream/:id", controllers.Stream)

	e.Start(":1234")
}

func dbInjector(db *gorm.DB) echo.MiddlewareFunc {
	return func(next echo.HandlerFunc) echo.HandlerFunc {
		return func(c echo.Context) error {
			c.Set("db", db)
			return next(c)
		}
	}
}
