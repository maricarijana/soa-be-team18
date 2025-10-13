package service

import (
	"fmt"
	"log"
	"net/http"
	"strconv"

	stakeholders "soa/follower/proto/stakeholders"

	"github.com/gin-gonic/gin"
	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
	"go.opentelemetry.io/otel"
	"go.opentelemetry.io/otel/codes"
)

var tracer = otel.Tracer("follower-service")

type FollowerService struct {
	Driver             neo4j.DriverWithContext
	StakeholdersClient stakeholders.StakeholdersServiceClient
}

func NewFollowerService(driver neo4j.DriverWithContext, client stakeholders.StakeholdersServiceClient) *FollowerService {
	return &FollowerService{Driver: driver, StakeholdersClient: client}
}

// POST /follow
func (s *FollowerService) FollowUser(c *gin.Context) {
	ctx, span := tracer.Start(c.Request.Context(), "FollowUser")
	defer span.End()

	var req struct {
		FollowerId int64 `json:"followerId"`
		FolloweeId int64 `json:"followeeId"`
	}
	log.Printf("Parsed request: followerId=%d, followeeId=%d", req.FollowerId, req.FolloweeId)

	if err := c.BindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	log.Printf("FollowUser: %d -> %d", req.FollowerId, req.FolloweeId)

	// 1. Pozovi Stakeholders servis
	accountsResp, err := s.StakeholdersClient.GetAllAccounts(ctx, &stakeholders.PagedRequest{Page: 1, PageSize: 1000})
	if err != nil {
		span.RecordError(err)
		span.SetStatus(codes.Error, "Stakeholders service failed")
		c.JSON(http.StatusInternalServerError, gin.H{"error": "stakeholders service unavailable"})
		return
	}

	// 2. Validacija ID-jeva
	followerExists, followeeExists := false, false
	for _, acc := range accountsResp.Accounts {
		if acc.Id == req.FollowerId {
			followerExists = true
		}
		if acc.Id == req.FolloweeId {
			followeeExists = true
		}
	}
	if !followerExists || !followeeExists {
		c.JSON(http.StatusBadRequest, gin.H{"error": "invalid user IDs"})
		return
	}

	// 3. Upis u Neo4j
	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeWrite})
	defer session.Close(ctx)

	_, err = session.ExecuteWrite(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		_, err := tx.Run(ctx, `
			MERGE (a:User {id: $followerId})
			MERGE (b:User {id: $followeeId})
			MERGE (a)-[:FOLLOWS]->(b)`,
			map[string]any{"followerId": req.FollowerId, "followeeId": req.FolloweeId})
		return nil, err
	})
	if err != nil {
		span.RecordError(err)
		c.JSON(http.StatusInternalServerError, gin.H{"error": "database error"})
		return
	}

	c.JSON(http.StatusOK, gin.H{"message": "Follow saved in Neo4j"})
}

// POST /unfollow
func (s *FollowerService) UnfollowUser(c *gin.Context) {
	ctx, span := tracer.Start(c.Request.Context(), "UnfollowUser")
	defer span.End()

	var req struct {
		FollowerId int64 `json:"followerId"`
		FolloweeId int64 `json:"followeeId"`
	}
	if err := c.BindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeWrite})
	defer session.Close(ctx)

	_, err := session.ExecuteWrite(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		_, err := tx.Run(ctx, `
			MATCH (a:User {id: $followerId})-[r:FOLLOWS]->(b:User {id: $followeeId})
			DELETE r`,
			map[string]any{"followerId": req.FollowerId, "followeeId": req.FolloweeId})
		return nil, err
	})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "failed to unfollow"})
		return
	}

	c.JSON(http.StatusOK, gin.H{"message": "Unfollowed successfully"})
}

// GET /following/:id
func (s *FollowerService) GetFollowing(c *gin.Context) {
	fmt.Println("🔥🔥🔥 Entered GetFollowing endpoint")
	ctx, span := tracer.Start(c.Request.Context(), "GetFollowing")
	defer span.End()

	userId, _ := strconv.ParseInt(c.Param("id"), 10, 64)

	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeRead})
	defer session.Close(ctx)

	var ids []int64
	_, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		result, err := tx.Run(ctx,
			`MATCH (a:User {id: $userId})-[:FOLLOWS]->(b:User)
			 RETURN b.id AS id`, map[string]any{"userId": userId})
		if err != nil {
			return nil, err
		}
		for result.Next(ctx) {
			if id, ok := result.Record().Values[0].(int64); ok {
				ids = append(ids, id)
			}
		}
		return nil, result.Err()
	})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.JSON(http.StatusOK, gin.H{"following": ids})
}

// GET /followers/:id
// GET /followers/:id
func (s *FollowerService) GetFollowers(c *gin.Context) {
	fmt.Println("🔥🔥🔥 Entered GetFollowing endpoint")
	ctx, span := tracer.Start(c.Request.Context(), "GetFollowers")
	defer span.End()

	// 📍 loguj ulazak u endpoint
	log.Printf("➡️  %s %s", c.Request.Method, c.Request.URL.Path)

	userId, _ := strconv.ParseInt(c.Param("id"), 10, 64)
	log.Printf("🔎 Fetching followers for userId=%d", userId)

	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeRead})
	defer session.Close(ctx)

	var ids []int64
	_, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		result, err := tx.Run(ctx,
			`MATCH (a:User)-[:FOLLOWS]->(b:User {id: $userId})
			 RETURN a.id AS id`, map[string]any{"userId": userId})
		if err != nil {
			log.Printf("❌ Neo4j query error in GetFollowers: %v", err)
			return nil, err
		}
		for result.Next(ctx) {
			if id, ok := result.Record().Values[0].(int64); ok {
				log.Printf("🧩 Found follower ID=%d for user %d", id, userId)
				ids = append(ids, id)
			}
		}
		if result.Err() != nil {
			log.Printf("⚠️ Result iteration error: %v", result.Err())
		}
		return nil, result.Err()
	})
	if err != nil {
		log.Printf("❌ Database read failed in GetFollowers: %v", err)
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	log.Printf("✅ Returning followers for user %d: %v", userId, ids)
	c.JSON(http.StatusOK, gin.H{"followers": ids})
}

// GET /recommendations/:id
func (s *FollowerService) GetRecommendations(c *gin.Context) {
	ctx, span := tracer.Start(c.Request.Context(), "GetRecommendations")
	defer span.End()

	userId, _ := strconv.ParseInt(c.Param("id"), 10, 64)
	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeRead})
	defer session.Close(ctx)

	var ids []int64
	_, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		result, err := tx.Run(ctx, `
			MATCH (me:User {id: $userId})-[:FOLLOWS]->(friend:User)-[:FOLLOWS]->(rec:User)
			WHERE NOT (me)-[:FOLLOWS]->(rec) AND rec.id <> $userId
			RETURN DISTINCT rec.id AS id LIMIT 10`,
			map[string]any{"userId": userId})
		if err != nil {
			return nil, err
		}
		for result.Next(ctx) {
			if id, ok := result.Record().Values[0].(int64); ok {
				ids = append(ids, id)
			}
		}
		return nil, result.Err()
	})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.JSON(http.StatusOK, gin.H{"userIds": ids})
}
