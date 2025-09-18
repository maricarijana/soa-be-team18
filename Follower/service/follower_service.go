package service

import (
	"context"
	"log"
	follower "soa/follower/proto/follower"

	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
)

// FollowerServer implementira interfejs generisan iz follower_grpc.pb.go
type FollowerServer struct {
    follower.UnimplementedFollowerServiceServer
	Driver neo4j.DriverWithContext
}

// FollowUser – trenutno samo testna implementacija
func (s *FollowerServer) FollowUser(ctx context.Context, req *follower.FollowRequest) (*follower.FollowResponse, error) {
	log.Printf("FollowUser: %d -> %d", req.FollowerId, req.FolloweeId)

	// primer upisa u Neo4j
	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeWrite}) //otvaranje sesije (uvijek se pravi nova sesija za svaku operaciju) i biramo AccessMode WRITE kad upisujemo u bazu
	defer session.Close(ctx)

	_, err := session.ExecuteWrite(ctx, func(tx neo4j.ManagedTransaction) (any, error) { //radim transkaciju (query se izvrsava u write transaction i automatski retry ako nesto ne uspije)
		_, err := tx.Run(ctx, //MERGE znači "nađi ako postoji, napravi ako ne postoji"
			`MERGE (a:User {id: $followerId}) 
			 MERGE (b:User {id: $followeeId})
			 MERGE (a)-[:FOLLOWS]->(b)`,
			map[string]any{
				"followerId": req.FollowerId,
				"followeeId": req.FolloweeId,
			})
		return nil, err
	})
	if err != nil {
		return nil, err
	}

	return &follower.FollowResponse{Message: "Follow saved in Neo4j"}, nil
}

func (s *FollowerServer) UnfollowUser(ctx context.Context, req *follower.FollowRequest) (*follower.FollowResponse, error) {
	log.Printf("UnfollowUser: %d unfollows %d", req.FollowerId, req.FolloweeId)

	// Otvaramo session
	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeWrite})
	defer session.Close(ctx)

	// Brišemo relaciju
	_, err := session.ExecuteWrite(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		_, err := tx.Run(ctx,
			`MATCH (a:User {id: $followerId})-[r:FOLLOWS]->(b:User {id: $followeeId})
			 DELETE r`,
			map[string]any{
				"followerId": req.FollowerId,
				"followeeId": req.FolloweeId,
			})
		return nil, err
	})

	if err != nil {
		return nil, err
	}

	return &follower.FollowResponse{Message: "Unfollowed successfully"}, nil
}


func (s *FollowerServer) GetFollowing(ctx context.Context, req *follower.UserRequest) (*follower.UserListResponse, error) {
	log.Printf("GetFollowing for user: %d", req.UserId)
	return &follower.UserListResponse{UserIds: []int64{101, 102}}, nil
}

func (s *FollowerServer) GetFollowers(ctx context.Context, req *follower.UserRequest) (*follower.UserListResponse, error) {
	log.Printf("GetFollowers for user: %d", req.UserId)
	return &follower.UserListResponse{UserIds: []int64{201, 202}}, nil
}

func (s *FollowerServer) GetRecommendations(ctx context.Context, req *follower.UserRequest) (*follower.UserListResponse, error) {
	log.Printf("GetRecommendations for user: %d", req.UserId)
	return &follower.UserListResponse{UserIds: []int64{301, 302}}, nil
}