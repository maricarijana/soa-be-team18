package model

import (
	"fmt"
	"time"

	"go.mongodb.org/mongo-driver/bson/primitive"
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
	ID primitive.ObjectID `json:"id" bson:"_id,omitempty"`

	Title       string     `json:"title"`
	Description string     `json:"description"`
	CreatedAt   time.Time  `json:"createdAt"`
	ImageUrl    string     `json:"imageUrl"`
	Status      BlogStatus `json:"status"`
	UserId      int64      `json:"userId"`
	RatingSum   int        `json:"ratingSum"`
	Ratings     Ratings    `json:"ratings" gorm:"type:jsonb"`
	Comments    []Comment  `json:"comments" bson:"comments"`
}

func (blog *Blog) AddRating(value int, userId int64) error {
	for _, rating := range blog.Ratings {
		if rating.UserID == userId {
			return fmt.Errorf("Rating from this user already exists")
		}
	}
	blog.Ratings = append(blog.Ratings, Rating{
		UserID:    userId,
		Value:     value,
		CreatedAt: time.Now(),
	})
	blog.RecalculateRatingSum()
	return nil
}

func (blog *Blog) RemoveRating(userId int64) {
	newRatings := make([]Rating, 0)
	for _, rating := range blog.Ratings {
		if rating.UserID != userId {
			newRatings = append(newRatings, rating)
		}
	}
	blog.Ratings = newRatings
	blog.RecalculateRatingSum()
}

func (blog *Blog) RecalculateRatingSum() {
	sum := 0
	for _, rating := range blog.Ratings {
		sum += rating.Value
	}
	blog.RatingSum = sum
}
