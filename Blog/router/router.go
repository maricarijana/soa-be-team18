package router

import (
	"blog/handler"

	"github.com/gorilla/mux"
)

func SetupRouter(blogHandler *handler.BlogHandler,  commentHandler *handler.CommentHandler) *mux.Router {
	router := mux.NewRouter()

	router.HandleFunc("/blogs", blogHandler.Create).Methods("POST")
	router.HandleFunc("/blogs/{blogId}/comments", commentHandler.Create).Methods("POST")

	return router
}
