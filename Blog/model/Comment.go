package model

import (
	"time"

	"go.mongodb.org/mongo-driver/bson/primitive"
)



type Comment struct {
	ID primitive.ObjectID `json:"_id" bson:"_id,omitempty"`
	Text      string    `json:"text"`
	CreatedAt time.Time `json:"createdAt"`
	UpdatedAt time.Time `json:"updatedAt"`
	UserId    int64     `json:"userId"`
	BlogId primitive.ObjectID `json:"blogId" bson:"blogId"`
	Username  string    `json:"username"`
}
