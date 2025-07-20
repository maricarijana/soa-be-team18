package main

import (
	//"github.com/gorilla/mux"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
	"blog/model"
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
	
	initDB()

}