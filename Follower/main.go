package main

import (
	"context"
	"log"
	"net"
	"os"

	"soa/follower/proto/follower"
	stakeholders "soa/follower/proto/stakeholders"
	"soa/follower/service"

	"google.golang.org/grpc/credentials/insecure"

	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
	"google.golang.org/grpc"
)

func main() {
	// --- 1. Procitaj env varijable ---

		tp, err := initTracer()
if err != nil {
    log.Fatalf("Failed to initialize tracer: %v", err)
}
defer func() {
    if err := tp.Shutdown(context.Background()); err != nil {
        log.Fatalf("Failed to shutdown tracer: %v", err)
    }
}()

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
	defer driver.Close(context.Background())


	// --- 3. Pokreni gRPC server ---
	lis, err := net.Listen("tcp", ":9091")
	if err != nil {
		log.Fatalf("Failed to listen: %v", err)
	}

	grpcServer := grpc.NewServer()

	connStakeholders, err := grpc.DialContext(
    context.Background(),
    "stakeholders-service:80",
    grpc.WithTransportCredentials(insecure.NewCredentials()),
)
if err != nil {
    log.Fatalf("Failed to connect to stakeholders-service: %v", err)
}
	stakeholdersClient := stakeholders.NewStakeholdersServiceClient(connStakeholders)
	// Prosledi driver u servis
	follower.RegisterFollowerServiceServer(grpcServer, &service.FollowerServer{
		Driver: driver,
		StakeholdersClient: stakeholdersClient,
	})

	log.Println("Follower gRPC server started on :9091")

	if err := grpcServer.Serve(lis); err != nil {
		log.Fatalf("Failed to serve: %v", err)
	}

}
