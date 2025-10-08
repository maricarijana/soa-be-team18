package model

import (
	"time"

	"go.mongodb.org/mongo-driver/bson/primitive"
)

type Comment struct {
	ID        primitive.ObjectID `json:"_id"       bson:"_id,omitempty"`
	Text      string             `json:"text"      bson:"text"`
	CreatedAt time.Time          `json:"createdAt" bson:"createdAt"`
	UpdatedAt time.Time          `json:"updatedAt" bson:"updatedAt"`
	UserId    int64              `json:"userId"    bson:"userId"`
	BlogId    primitive.ObjectID `json:"blogId"    bson:"blogId"`
	Username  string             `json:"username"  bson:"username"`
}
