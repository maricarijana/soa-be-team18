package repository

import (
	"soa/blog/model"

	"go.mongodb.org/mongo-driver/bson/primitive"
)

type CommentRepository interface {
	Create(comment *model.Comment) error
	GetByID(id primitive.ObjectID) (*model.Comment, error)
	GetByBlogID(blogId primitive.ObjectID) ([]model.Comment, error) 
	GetAll() ([]model.Comment, error)
	Update(comment *model.Comment) error
	Delete(id primitive.ObjectID) error
}