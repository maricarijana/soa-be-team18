package repository

import (
	"blog/model"

	"gorm.io/gorm"
)

type BlogRepositoryImpl struct {
	DbConnection *gorm.DB
}

func (r *BlogRepositoryImpl) Create(blog *model.Blog) error {
	return r.DbConnection.Create(blog).Error
}

func (r *BlogRepositoryImpl) GetByID(id int64) (*model.Blog, error) {
	var blog model.Blog
	if err := r.DbConnection.Preload("Comments").First(&blog, id).Error; err != nil {
		return nil, err
	}
	return &blog, nil
}

func (r *BlogRepositoryImpl) GetAll() ([]model.Blog, error) {
	var blogs []model.Blog
	if err := r.DbConnection.Preload("Comments").Find(&blogs).Error; err != nil {
		return nil, err
	}
	return blogs, nil
}

func (r *BlogRepositoryImpl) Update(blog *model.Blog) error {
	return r.DbConnection.Save(blog).Error
}

func (r *BlogRepositoryImpl) Delete(id int64) error {
	return r.DbConnection.Delete(&model.Blog{}, id).Error
}
