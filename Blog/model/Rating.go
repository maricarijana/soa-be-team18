package model

import (
	"time"
)

type Rating struct {
	UserID    int64     `json:"userId"   bson:"userId"`
	Value     int       `json:"value"    bson:"value"`
	CreatedAt time.Time `json:"createdAt" bson:"createdAt"`
}
