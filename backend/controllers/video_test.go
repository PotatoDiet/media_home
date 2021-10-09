package controllers

import (
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"potatodiet/media_home/items"
	"testing"

	"github.com/DATA-DOG/go-sqlmock"
	"github.com/labstack/echo/v4"
	"github.com/stretchr/testify/assert"
	"gorm.io/driver/mysql"
	"gorm.io/gorm"
)

func Setup(t *testing.T) (Controller, *httptest.ResponseRecorder, echo.Context, sqlmock.Sqlmock) {
	db, mock, err := sqlmock.New()
	if err != nil {
		t.Fatalf("error opening mock db.")
	}

	gdb, err := gorm.Open(mysql.New(mysql.Config{
		Conn:                      db,
		SkipInitializeWithVersion: true,
	}), &gorm.Config{SkipDefaultTransaction: true})

	e := echo.New()
	req := httptest.NewRequest(echo.GET, "/", nil)
	rec := httptest.NewRecorder()
	ctx := e.NewContext(req, rec)

	controller := Controller{
		DB: gdb,
	}

	return controller, rec, ctx, mock
}

func TestVideo(t *testing.T) {
	controller, rec, ctx, mock := Setup(t)

	columns := []string{
		"id",
		"title",
		"year",
		"location",
		"genres",
		"community_rating",
		"poster",
		"current_watch_timestamp",
	}
	rows := sqlmock.NewRows(columns).AddRow(
		1, "Big Buck Bunny", "2008", "mocks/Big Buck Bunny (2008).mp4", "Animation, Comedy, Family", 6.5, "/posters/1323213.png", 3)
	mock.ExpectQuery("SELECT (.+) FROM `videos`").WillReturnRows(rows)

	expectedResponse := items.Video{
		ID:                    1,
		Title:                 "Big Buck Bunny",
		Year:                  "2008",
		Location:              "mocks/Big Buck Bunny (2008).mp4",
		Genres:                "Animation, Comedy, Family",
		CommunityRating:       6.5,
		Poster:                "/posters/1323213.png",
		CurrentWatchTimestamp: 3,
	}
	actualResponse := items.Video{}

	ctx.Set("id", 1)
	controller.Video(ctx)

	err := json.Unmarshal(rec.Body.Bytes(), &actualResponse)
	assert.Nil(t, err)
	assert.Equal(t, expectedResponse, actualResponse)
}

func TestVideoUpdateWatchTimestamp(t *testing.T) {
	controller, rec, ctx, mock := Setup(t)

	columns := []string{
		"id",
		"title",
		"year",
		"location",
		"genres",
		"community_rating",
		"poster",
		"current_watch_timestamp",
	}
	rows := sqlmock.NewRows(columns).
		AddRow(1, "Big Buck Bunny", "2008", "mocks/Big Buck Bunny (2008).mp4", "Animation, Comedy, Family", 6.5, "/posters/1323213.png", 3)
	mock.ExpectQuery("SELECT (.+) FROM `videos`").WillReturnRows(rows)
	mock.ExpectExec("UPDATE `videos`").WithArgs("Big Buck Bunny", "2008", "mocks/Big Buck Bunny (2008).mp4", "Animation, Comedy, Family", 6.5, "/posters/1323213.png", 10, 1).WillReturnResult(sqlmock.NewResult(1, 1))

	ctx.Set("id", 1)
	ctx.QueryParams().Set("timestamp", "10")
	controller.VideoUpdateWatchTimestamp(ctx)

	assert.Equal(t, http.StatusOK, rec.Result().StatusCode)
	assert.Nil(t, mock.ExpectationsWereMet())
}
