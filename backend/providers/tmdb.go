package providers

import (
	"io"
	"net/http"
	"os"
	"path/filepath"
	"potatodiet/media_home/items"
	"regexp"

	gotmdb "github.com/cyruzin/golang-tmdb"
)

// Tmdb wraps the underlying tmdb client library.
type Tmdb struct {
	client gotmdb.Client
}

// TmdbInit creates and returns a new Tmdb client.
func TmdbInit(key string) *Tmdb {
	client, _ := gotmdb.Init(key)
	return &Tmdb{client: *client}
}

// Find fills in the provided video's metadata with the info found on tmdb.
func (p Tmdb) Find(video *items.Video) error {
	title, year := extractDetails(video.Location)

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

	downloadFile(
		"https://image.tmdb.org/t/p/original"+res.Results[0].PosterPath,
		video.Poster,
	)

	details, err := p.client.GetMovieDetails(int(res.Results[0].ID), nil)
	if err != nil {
		return err
	}
	video.CommunityRating = details.VoteAverage
	for _, genre := range details.Genres {
		video.Genres = append(video.Genres, genre.Name)
	}

	return nil
}

func downloadFile(url string, path string) error {
	out, err := os.Create(path)
	if err != nil {
		return err
	}
	defer out.Close()

	res, err := http.Get(url)
	if err != nil {
		return err
	}
	defer res.Body.Close()

	_, err = io.Copy(out, res.Body)
	return err
}

// extracts title and year
// path should be in the form /some/dir/The Title (Year).mkv
func extractDetails(path string) (string, string) {
	re := regexp.MustCompile(`(.+) \((\d+)\).+`)
	match := re.FindStringSubmatch(filepath.Base(path))
	return match[1], match[2]
}
