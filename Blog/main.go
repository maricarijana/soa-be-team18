package main

import (
	//"github.com/gorilla/mux"
	"blog/handler"
	"blog/model"
	"blog/repository"
	"blog/router"
	"blog/service"
	"log"
	"net/http"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func initDB() *gorm.DB {
	//host ce biti drugaciji kad bude docker
	connectionURL := "user=postgres password=super dbname=blog-service host=localhost port=5432 sslmode=disable"
	database, err := gorm.Open(postgres.Open(connectionURL), &gorm.Config{})

	if err != nil {
		println("Failed to connect to database:")
		println(err.Error())
		return nil
	}
	println("Successful connection")
	database.AutoMigrate(&model.Blog{}, &model.Comment{})

	return database
}

func main() {

	db := initDB()

	blogRepo := &repository.BlogRepositoryImpl{DbConnection: db}
	blogService := &service.BlogService{BlogRepository: blogRepo}
	blogHandler := &handler.BlogHandler{BlogService: blogService}

	commentRepo := &repository.CommentRepositoryImpl{DbConnection: db}
	commentService := &service.CommentService{CommentRepository: commentRepo}
	commentHandler := &handler.CommentHandler{CommentService: commentService}

	router := router.SetupRouter(blogHandler, commentHandler)
	log.Println("Server running on http://localhost:8082")

	log.Fatal(http.ListenAndServe(":8082", router))
}
