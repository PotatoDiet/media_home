package providers

import (
  "potatodiet/media_home_backend/items"
)

type Provider interface {
  Find(video *items.Video) error
}
