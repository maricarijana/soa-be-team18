package router

import (
	"blog/handler"

	"github.com/gorilla/mux"
)

func SetupRouter(blogHandler *handler.BlogHandler) *mux.Router {
	router := mux.NewRouter()

	router.HandleFunc("/blogs", blogHandler.Create).Methods("POST")

	return router
}
