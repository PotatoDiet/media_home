package controllers

import (
	"gorm.io/gorm"
)

// Controller contains shared data used within each of the controller package methods.
type Controller struct {
	DB *gorm.DB
}
