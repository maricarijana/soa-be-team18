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
	// Migracija tabele, otkom kad bude modela
	database.AutoMigrate(&model.Blog{},&model.Comment{})

	return database
}


func main(){
	
	db :=initDB()

	// Repository (implementacija)
	blogRepo := &repository.BlogRepositoryImpl{DbConnection: db}
	// Service
	blogService := &service.BlogService{BlogRepository: blogRepo}
	blogHandler := &handler.BlogHandler{BlogService: blogService}
	router := router.SetupRouter(blogHandler)
	log.Fatal(http.ListenAndServe(":8082",router))
}