package repository

import "blog/model"

type CommentRepository interface {
	Create(comment *model.Comment) error
	GetByID(id int64) (*model.Comment, error)
	GetByBlogID(blogId int64) ([]model.Comment, error) 
	GetAll() ([]model.Comment, error)
	Update(comment *model.Comment) error
	Delete(id int64) error
}