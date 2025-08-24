package service

import (
	"blog/model"
	"blog/repository"
	"time"
)

type CommentService struct {
	CommentRepository repository.CommentRepository
}

func (service *CommentService) Create(comment *model.Comment) error {
	return service.CommentRepository.Create(comment)
}

func (service *CommentService) GetByID(id int64) (*model.Comment, error) {
	return service.CommentRepository.GetByID(id)
}


func (service *CommentService) GetByBlogID(blogId int64) ([]model.Comment, error) {
	return service.CommentRepository.GetByBlogID(blogId)
}


func (service *CommentService) GetAll() ([]model.Comment, error) {
	return service.CommentRepository.GetAll()
}

func (service *CommentService) Update(comment *model.Comment) error {
	comment.UpdatedAt = time.Now()
	return service.CommentRepository.Update(comment)
}

func (service *CommentService) Delete(id int64) error {
	return service.CommentRepository.Delete(id)
}