package service

import (
	"soa/blog/model"
	"soa/blog/repository"

	"go.mongodb.org/mongo-driver/bson/primitive"
)

type BlogService struct {
	//bez zvezdice jer je ovo interfejs, a interfejs ima vec * u sebi
	BlogRepository repository.BlogRepository
	CommentRepository repository.CommentRepository
}

func (service *BlogService) Create(blog *model.Blog) error {
	err := service.BlogRepository.Create(blog)
	return err
}

func (service *BlogService) LikeBlog(blogId primitive.ObjectID, userId int64) error {
	blog, err := service.BlogRepository.GetByID(blogId)
	if err != nil {
		return err
	}

	if err := blog.AddRating(1, userId); err != nil {
		return err
	}

	return service.BlogRepository.Update(blog)
}

func (service *BlogService) GetByIDWithComments(id primitive.ObjectID) (*model.Blog, error) {
    blog, err := service.BlogRepository.GetByID(id)
    if err != nil {
        return nil, err
    }

    // dohvatimo sve komentare za ovaj blog
    comments, err := service.CommentRepository.GetByBlogID(id)
    if err == nil { 
        blog.Comments = comments
    }

    return blog, nil
}
