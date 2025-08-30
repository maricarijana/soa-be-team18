package repository

import (
	"soa/blog/model"

	"gorm.io/gorm"
)

type CommentRepositoryImpl struct {
	DbConnection *gorm.DB
}

func (r *CommentRepositoryImpl) Create(comment *model.Comment) error {
	return r.DbConnection.Create(comment).Error
}


func (r *CommentRepositoryImpl) GetAll() ([]model.Comment, error) {
	var comments []model.Comment
	err := r.DbConnection.Find(&comments).Error
	return comments, err
}

func (r *CommentRepositoryImpl) GetByID(id int64) (*model.Comment, error) {
	var comment model.Comment
	err := r.DbConnection.First(&comment, id).Error
	if err != nil {
		return nil, err
	}
	return &comment, nil
}

func (r *CommentRepositoryImpl) GetByBlogID(blogId int64) ([]model.Comment, error) {
	var comments []model.Comment
	err := r.DbConnection.Where("blog_id = ?", blogId).Find(&comments).Error
	return comments, err
}

func (r *CommentRepositoryImpl) Update(comment *model.Comment) error {
	return r.DbConnection.Save(comment).Error
}

func (r *CommentRepositoryImpl) Delete(id int64) error {
	return r.DbConnection.Delete(&model.Comment{}, id).Error
}