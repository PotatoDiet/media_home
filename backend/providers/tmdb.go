package providers

import (
  gotmdb "github.com/cyruzin/golang-tmdb"
  "potatodiet/media_home_backend/items"
  "net/http"
  "os"
  "io"
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
  video.Poster = "assets/posters" + res.Results[0].PosterPath
  video.Year = year

  DownloadFile(
    "https://image.tmdb.org/t/p/original" + res.Results[0].PosterPath,
    video.Poster,
  )

  return nil
}

func DownloadFile(url string, path string) {
  out, _ := os.Create(path)
  defer out.Close()

  res, _ := http.Get(url)
  defer res.Body.Close()

  io.Copy(out, res.Body)
}
