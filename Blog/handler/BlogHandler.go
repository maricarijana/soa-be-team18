package handler

import (
	"encoding/json"
	"fmt"
	"net/http"
	"soa/blog/model"
	"soa/blog/service"

	"github.com/gorilla/mux"
	"go.mongodb.org/mongo-driver/bson/primitive"
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

	fmt.Println("🟢 Blog received:", blog.Title)
	fmt.Println("🟢 ImageBase64 length:", len(blog.ImageBase64))
	if blog.ImageBase64 != "" {
		imageService := service.NewImageService("./wwwroot")
		imagePath, err := imageService.SaveBase64Image(blog.ImageBase64, "blogs")
		if err != nil {
			fmt.Println("❌ Error saving image:", err)
			http.Error(w, "Failed to save image: "+err.Error(), http.StatusInternalServerError)
			return
		}
		fmt.Println("✅ Image saved at:", imagePath)
		blog.ImageUrl = imagePath
	} else {
		fmt.Println("⚠️ blog.ImageBase64 je PRAZAN")
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

	// Konverzija u ObjectID
	objID, err := primitive.ObjectIDFromHex(blogIDStr)
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

	if err := handler.BlogService.LikeBlog(objID, request.UserID); err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	w.WriteHeader(http.StatusOK)
	w.Write([]byte(`{"message":"Blog liked successfully"}`))
}

