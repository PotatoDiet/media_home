package items

// Video represents a video and all it's metadata.
type Video struct {
	ID                    uint    `json:"id"`
	Title                 string  `json:"title"`
	Year                  string  `json:"year"`
	Location              string  `json:"location"`
	Genres                string  `json:"genres"`
	CommunityRating       float32 `json:"communityRating"`
	Poster                string  `json:"poster"`
	CurrentWatchTimestamp uint64  `json:"currentWatchTimestamp"`
}
