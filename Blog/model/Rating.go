package model

import "time"

type Rating struct {
	UserID    int64     `json:"userId"`
	Value     int       `json:"value"`
	CreatedAt time.Time `json:"createdAt"`
}
