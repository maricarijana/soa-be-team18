package handler

import (
	"blog/model"
	"blog/service"
	"encoding/json"
	"fmt"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
)

type BlogHandler struct {
	BlogService *service.BlogService
}

func (handler *BlogHandler) Create(w http.ResponseWriter, r *http.Request) {
	var blog model.Blog
	if err := json.NewDecoder(r.Body).Decode(&blog); err != nil {
		http.Error(w, "Invalid JSON format", http.StatusBadRequest)
		return
	}

	if err := handler.BlogService.Create(&blog); err != nil {
		http.Error(w, "Error while creating new blog", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
	json.NewEncoder(w).Encode(blog)
}

func (handler *BlogHandler) LikeBlog(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	blogIDStr := vars["blogId"]
	blogID, err := strconv.ParseInt(blogIDStr, 10, 64)
	fmt.Print(blogID)

	if err != nil {
		http.Error(w, "Invalid blog ID", http.StatusBadRequest)
		return
	}
	var request struct {
		UserID int64 `json:"userId"`
	}

	if err := json.NewDecoder(r.Body).Decode(&request); err != nil {
		http.Error(w, "Invalid JSON format", http.StatusBadRequest)
		return
	}

	if err := handler.BlogService.LikeBlog(blogID, request.UserID); err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	w.WriteHeader(http.StatusOK)
	w.Write([]byte(`{"message":"Blog liked successfully"}`))
}
