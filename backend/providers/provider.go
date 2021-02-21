package providers

import (
	"potatodiet/media_home_backend/items"
)

// Provider is the common interface used for all metadata providers.
type Provider interface {
	Find(video *items.Video) error
}
