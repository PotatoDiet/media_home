package items

type Video struct {
  ID uint `json:"id"`
  Title string `json:"title"`
  Year string `json:"year"`
  Location string `json:"location"`
  Genres string `json:"genres"`
  CommunityRating float32 `json:"communityRating"`
  Poster string `json:"poster"`
}
