package repository

import "blog/model"

type BlogRepository interface {
	Create(blog *model.Blog) error
	GetByID(id int64) (*model.Blog, error)
	GetAll() ([]model.Blog, error)
	Update(blog *model.Blog) error
	Delete(id int64) error
}
