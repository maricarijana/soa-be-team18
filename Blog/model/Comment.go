package model

import "time"

type Comment struct {
	ID        int64     `json:"id" gorm:"primaryKey"`
	Text      string    `json:"text"`
	CreatedAt time.Time `json:"createdAt"`
	UpdatedAt time.Time `json:"updatedAt"`
	UserId    int64     `json:"userId"`
	BlogId    int64     `json:"blogId"`
	Username  string    `json:"username"`
}
