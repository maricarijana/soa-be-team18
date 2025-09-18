package main

import (
	"log"
	"net"
	"os"

	"soa/follower/proto/follower"
	"soa/follower/service"

	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
	"google.golang.org/grpc"
)

func main() {
	// --- 1. Procitaj env varijable ---
	neo4jUri := os.Getenv("NEO4J_URI")
	neo4jUser := os.Getenv("NEO4J_USER")
	neo4jPassword := os.Getenv("NEO4J_PASSWORD")

	// --- 2. Konektuj se na Neo4j ---
	driver, err := neo4j.NewDriverWithContext(
		neo4jUri,
		neo4j.BasicAuth(neo4jUser, neo4jPassword, ""),
	)
	if err != nil {
		log.Fatalf("Failed to connect to Neo4j: %v", err)
	}
	defer driver.Close(nil)

	// --- 3. Pokreni gRPC server ---
	lis, err := net.Listen("tcp", ":9091")
	if err != nil {
		log.Fatalf("Failed to listen: %v", err)
	}

	grpcServer := grpc.NewServer()

	// Prosledi driver u servis
	follower.RegisterFollowerServiceServer(grpcServer, &service.FollowerServer{
		Driver: driver,
	})

	log.Println("Follower gRPC server started on :9091")

	if err := grpcServer.Serve(lis); err != nil {
		log.Fatalf("Failed to serve: %v", err)
	}
}
