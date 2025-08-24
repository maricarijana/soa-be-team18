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
	"time"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

//host=localhost ako pokrecemo kod sebe
func initDB() *gorm.DB {
	connectionURL := "user=postgres password=super dbname=blog-service host=database port=5432 sslmode=disable"

	var db *gorm.DB
	var err error

	for attempts := 1; attempts <= 10; attempts++ {
		db, err = gorm.Open(postgres.Open(connectionURL), &gorm.Config{})
		if err == nil {
			log.Println(" Connected to database")
			db.AutoMigrate(&model.Blog{}, &model.Comment{})
			return db
		}
		log.Printf("Attempt %d: Waiting for database...", attempts)
		time.Sleep(2 * time.Second)
	}

	log.Println("Could not connect to database after 10 attempts.")
	return nil
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
