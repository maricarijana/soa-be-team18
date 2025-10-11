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
	ID          primitive.ObjectID `json:"id"          bson:"_id,omitempty"`
	Title       string             `json:"title"       bson:"title"`
	Description string             `json:"description" bson:"description"`
	CreatedAt   time.Time          `json:"createdAt"   bson:"createdAt"`
	ImageUrl    string             `json:"imageUrl"    bson:"imageUrl"`
	ImageBase64 string             `json:"imageBase64" bson:"-"`
	Status      BlogStatus         `json:"status"      bson:"status"`
	UserId      int64              `json:"userId"      bson:"userId"`

	RatingSum int       `json:"ratingSum"   bson:"ratingSum"`
	Ratings   []Rating  `json:"ratings"     bson:"ratings,omitempty"`
	Comments  []Comment `json:"comments,omitempty" bson:"comments,omitempty"`
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
