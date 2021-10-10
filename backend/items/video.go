package items

import "gorm.io/gorm"

// Video represents a video and all it's metadata.
type Video struct {
	ID                    uint     `json:"id"`
	Title                 string   `json:"title"`
	Year                  string   `json:"year"`
	Location              string   `json:"location"`
	Genres                []string `json:"genres" gorm:"-"`
	CommunityRating       float32  `json:"communityRating"`
	Poster                string   `json:"poster"`
	CurrentWatchTimestamp uint64   `json:"currentWatchTimestamp"`
}

type Genre struct {
	ID   uint   `json:"id"`
	Name string `json:"name"`
}

func VideoGet(db *gorm.DB, id string) Video {
	video := Video{}
	video.Genres = []string{}
	db.First(&video, id)

	genres := []Genre{}
	db.Raw("select genres.name from genres join video_genres on genre_id = genres.id where video_id = ?", id).Find(&genres)

	for _, genre := range genres {
		video.Genres = append(video.Genres, genre.Name)
	}

	return video
}

func (video *Video) SaveGenres(db *gorm.DB) {
	tx := db.Begin()

	tx.Exec("delete from video_genres where video_id = ?", video.ID)

	for _, name := range video.Genres {
		tx.Exec("insert or ignore into genres (name) values (?)", name)

		var genre Genre
		tx.Where("name = ?", name).Find(&genre)

		tx.Exec("insert into video_genres (video_id, genre_id) values (?, ?)", video.ID, genre.ID)
	}

	tx.Commit()
}
