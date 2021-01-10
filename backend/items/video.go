package items

type Video struct {
  ID uint `json:"id"`
  Title string `json:"title"`
  Year string `json:"year"`
  Genre string `json:"genre"`
  Language string `json:"language"`
  Country string `json:"country"`
  Location string `json:"location"`
  Poster string `json:"poster"`
}
