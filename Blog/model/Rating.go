package model

import (
	"database/sql/driver"
	"encoding/json"
	"errors"
	"time"
)

type Rating struct {
	UserID    int64     `json:"userId"`
	Value     int       `json:"value"`
	CreatedAt time.Time `json:"createdAt"`
}

// added this because reading from db of jsonb wasnt working
type Ratings []Rating

func (r *Ratings) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return errors.New("type assertion to []byte failed")
	}
	return json.Unmarshal(bytes, r)
}

func (r Ratings) Value() (driver.Value, error) {
	return json.Marshal(r)
}
