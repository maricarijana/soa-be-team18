package handler

import (
	"blog/model"
	"blog/service"
	"encoding/json"
	"net/http"
)

type BlogHandler struct {
	BlogService *service.BlogService
}

func (handler *BlogHandler) Create(w http.ResponseWriter, r *http.Request){
	var blog model.Blog
	if err:= json.NewDecoder(r.Body).Decode(&blog); err!=nil{
		http.Error(w,"Invalid JSON format",http.StatusBadRequest)
		return
	}

	if err:= handler.BlogService.Create(&blog); err!=nil{
		http.Error(w,"Error while creating new blog",http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
	json.NewEncoder(w).Encode(blog)
}