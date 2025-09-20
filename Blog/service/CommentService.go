package service

import (
	"soa/blog/model"
	"soa/blog/repository"
	"time"

	"go.mongodb.org/mongo-driver/bson/primitive"
)

type CommentService struct {
	CommentRepository repository.CommentRepository
}

func (service *CommentService) Create(comment *model.Comment) error {
	return service.CommentRepository.Create(comment)
}

func (service *CommentService) GetByID(id primitive.ObjectID) (*model.Comment, error) {
	return service.CommentRepository.GetByID(id)
}


func (service *CommentService) GetByBlogID(blogId primitive.ObjectID) ([]model.Comment, error) {
	return service.CommentRepository.GetByBlogID(blogId)
}


func (service *CommentService) GetAll() ([]model.Comment, error) {
	return service.CommentRepository.GetAll()
}

func (service *CommentService) Update(comment *model.Comment) error {
	comment.UpdatedAt = time.Now()
	return service.CommentRepository.Update(comment)
}

func (service *CommentService) Delete(id primitive.ObjectID) error {
	return service.CommentRepository.Delete(id)
}