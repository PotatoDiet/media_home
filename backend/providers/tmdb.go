package providers

import (
  gotmdb "github.com/cyruzin/golang-tmdb"
  "potatodiet/media_home_backend/items"
)

type tmdb struct {
  client gotmdb.Client
}

func TmdbInit(key string) *tmdb {
  client, _ := gotmdb.Init(key)
  return &tmdb{client: *client}
}

func (p tmdb) Find(video *items.Video) error {
  title, year := extract_details(video.Location)

  options := map[string]string{
    "year": year,
  }

  res, err := p.client.GetSearchMovies(title, options)
  if err != nil {
    return err
  }
  video.Title = res.Results[0].Title
  video.Year = year

  return nil
}
