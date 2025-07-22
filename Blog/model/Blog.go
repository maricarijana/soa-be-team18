package model

import (
	"time"
)

type BlogStatus int

const (
	DRAFT BlogStatus = iota
	PUBLISHED
	CLOSED
	ACTIVE
	FAMOUS
)

type Blog struct {
	ID          int64      `json:"id" gorm:"primaryKey"`
	Title       string     `json:"title"`
	Description string     `json:"description"`
	CreatedAt   time.Time  `json:"createdAt"`
	ImageUrl    string     `json:"imageUrl"`
	Status      BlogStatus `json:"status"`
	UserId      int64      `json:"userId"`
	RatingSum   int        `json:"ratingSum"`
	Ratings     []Rating   `json:"ratings" gorm:"type:jsonb"`
	Comments    []Comment  `json:"comments" gorm:"foreignKey:BlogId;constraint:OnUpdate:CASCADE,OnDelete:CASCADE"`
}
