package repository

import (
	"soa/blog/model"

	"go.mongodb.org/mongo-driver/bson/primitive"
)


type BlogRepository interface {
	Create(blog *model.Blog) error
	GetByID(id primitive.ObjectID) (*model.Blog, error)
	GetAll() ([]model.Blog, error)
	Update(blog *model.Blog) error
	Delete(id int64) error
}
