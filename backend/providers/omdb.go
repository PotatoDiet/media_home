package providers

import (
  "github.com/eefret/gomdb"
  "potatodiet/media_home_backend/items"
  "path/filepath"
  "regexp"
)

type omdb struct {
  Api gomdb.OmdbApi
}

func OmdbInit(key string) *omdb {
  return &omdb{Api: *gomdb.Init(key)}
}

func (p omdb) Find(video *items.Video) error {
  title, year := extract_details(video.Location)
  query := &gomdb.QueryData{Title: title, Year: year}
	res, err := p.Api.MovieByTitle(query)
	if err != nil {
		return err
	}

  video.Title = res.Title
  video.Year = res.Year
  video.Genre = res.Genre
  video.Language = res.Language
  video.Country = res.Country

  return nil
}

// extracts title and year
// path should be in the form /some/dir/The Title (Year).mkv
func extract_details(path string) (string, string) {
  re := regexp.MustCompile(`(.+) \((\d+)\).+`)
  match := re.FindStringSubmatch(filepath.Base(path))
  return match[1], match[2]
}
