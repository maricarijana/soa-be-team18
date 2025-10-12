package main

import (
	"context"
	"log"
	"os"

	stakeholders "soa/follower/proto/stakeholders"
	"soa/follower/service"
	"soa/follower/tracing"

	"github.com/gin-gonic/gin"
	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
	"go.opentelemetry.io/contrib/instrumentation/github.com/gin-gonic/gin/otelgin"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

func main() {
	// --- 1. Tracing setup ---
	tp, err := tracing.InitTracer("follower-service")
	if err != nil {
		log.Fatalf("❌ Failed to initialize tracer: %v", err)
	}
	if tp != nil {
		defer func() {
			if err := tp.Shutdown(context.Background()); err != nil {
				log.Printf("⚠️ Failed to shutdown tracer: %v", err)
			}
		}()
	}

	// --- 2. Neo4j connection ---
	neo4jUri := os.Getenv("NEO4J_URI")
	neo4jUser := os.Getenv("NEO4J_USER")
	neo4jPassword := os.Getenv("NEO4J_PASSWORD")

	driver, err := neo4j.NewDriverWithContext(
		neo4jUri,
		neo4j.BasicAuth(neo4jUser, neo4jPassword, ""),
	)
	if err != nil {
		log.Fatalf("❌ Failed to connect to Neo4j: %v", err)
	}
	defer driver.Close(context.Background())

	// --- 3. Connect to Stakeholders (gRPC client) ---
	connStakeholders, err := grpc.DialContext(
		context.Background(),
		"stakeholders-service:80",
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalf("❌ Failed to connect to stakeholders-service: %v", err)
	}
	stakeholdersClient := stakeholders.NewStakeholdersServiceClient(connStakeholders)

	// --- 4. Init Follower REST service ---
	followerService := service.NewFollowerService(driver, stakeholdersClient)

	router := gin.Default()
	router.Use(otelgin.Middleware("follower-service"))

	// --- 5. REST routes ---
	router.POST("/follow", followerService.FollowUser)
	router.POST("/unfollow", followerService.UnfollowUser)
	router.GET("/followers/:id", followerService.GetFollowers)
	router.GET("/following/:id", followerService.GetFollowing)
	router.GET("/recommendations/:id", followerService.GetRecommendations)

	// --- 6. Run server ---
	port := os.Getenv("PORT")
	if port == "" {
		port = "9091"
	}
	log.Printf("🚀 Follower REST service running on :%s", port)
	router.Run(":" + port)
}
