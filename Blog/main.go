package main

import (
	//"github.com/gorilla/mux"
	"log"
	"net"

	grpcsrv "soa/blog/grpc"
	"soa/blog/model"
	"soa/blog/proto/blog"
	"soa/blog/repository"
	"soa/blog/service"
	"time"

	"google.golang.org/grpc"
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
commentRepo := &repository.CommentRepositoryImpl{DbConnection: db}

// Servisi
blogService := &service.BlogService{BlogRepository: blogRepo}
commentService := &service.CommentService{CommentRepository: commentRepo}

// gRPC server
lis, err := net.Listen("tcp", ":9090")
if err != nil {
	log.Fatalf("failed to listen: %v", err)
}

grpcServer := grpc.NewServer()
blog.RegisterBlogServiceServer(grpcServer, &grpcsrv.Server{
	BlogSvc:    blogService,
	CommentSvc: commentService,
})

log.Println("Blog gRPC server started on :9090")
if err := grpcServer.Serve(lis); err != nil {
	log.Fatalf("failed to serve: %v", err)
}
}