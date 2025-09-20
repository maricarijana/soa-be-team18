package handler

import (
	"encoding/json"
	"net/http"
	"soa/blog/model"
	"soa/blog/service"

	"github.com/gorilla/mux"
	"go.mongodb.org/mongo-driver/bson/primitive"
)

type CommentHandler struct {
	CommentService *service.CommentService
}

func (h *CommentHandler) Create(w http.ResponseWriter, r *http.Request) {
	var comment model.Comment

	// Uzmi blogId iz URL-a
	vars := mux.Vars(r)
	blogIdStr := vars["blogId"]
	blogId, err := primitive.ObjectIDFromHex(blogIdStr)
	if err != nil {
		http.Error(w, "Invalid blog ID", http.StatusBadRequest)
		return
	}
	comment.BlogId = blogId

	// Decode JSON
	if err := json.NewDecoder(r.Body).Decode(&comment); err != nil {
		http.Error(w, "Invalid JSON format", http.StatusBadRequest)
		return
	}

	if err := h.CommentService.Create(&comment); err != nil {
		http.Error(w, "Error while creating comment", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
	json.NewEncoder(w).Encode(comment)
}