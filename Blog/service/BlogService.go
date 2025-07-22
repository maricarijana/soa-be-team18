package service

import (
	"blog/model"
	"blog/repository"
)

type BlogService struct {
	//bez zvezdice jer je ovo interfejs, a interfejs ima vec * u sebi
	BlogRepository repository.BlogRepository
}

func (service *BlogService) Create(blog *model.Blog) error {
	err := service.BlogRepository.Create(blog)
	return err
}
