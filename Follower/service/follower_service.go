package service

import (
	"context"
	"fmt"
	"log"
	follower "soa/follower/proto/follower"
	stakeholders "soa/follower/proto/stakeholders"

	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
)

// FollowerServer implementira interfejs generisan iz follower_grpc.pb.go
type FollowerServer struct {
    follower.UnimplementedFollowerServiceServer
	Driver neo4j.DriverWithContext
	StakeholdersClient stakeholders.StakeholdersServiceClient
}

func (s *FollowerServer) FollowUser(ctx context.Context, req *follower.FollowRequest) (*follower.FollowResponse, error) {
    log.Printf("FollowUser: %d -> %d", req.FollowerId, req.FolloweeId)

    // 1. Pozovi Stakeholders servis da dohvati sve naloge
    accountsResp, err := s.StakeholdersClient.GetAllAccounts(ctx, &stakeholders.PagedRequest{Page: 1, PageSize: 1000})
    if err != nil {
        return nil, fmt.Errorf("failed to contact stakeholders service: %w", err)
    }

    // 2. Proveri da li oba ID-ja postoje u listi
    followerExists := false
    followeeExists := false

    for _, acc := range accountsResp.Accounts {
        if acc.Id == req.FollowerId {
            followerExists = true
        }
        if acc.Id == req.FolloweeId {
            followeeExists = true
        }
    }

    if !followerExists || !followeeExists {
        return nil, fmt.Errorf("invalid user IDs: follower=%d, followee=%d", req.FollowerId, req.FolloweeId)
    }

    // 3. Ako postoje, upiši u Neo4j
    session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeWrite})
    defer session.Close(ctx)

    _, err = session.ExecuteWrite(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
        _, err := tx.Run(ctx,
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

	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeRead})
	defer session.Close(ctx)

	userIds := []int64{}

	_, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		result, err := tx.Run(ctx,
			`MATCH (a:User {id: $userId})-[:FOLLOWS]->(b:User)
             RETURN b.id AS id`,
			map[string]any{
				"userId": req.UserId,
			})
		if err != nil {
			return nil, err
		}

		for result.Next(ctx) {
			if id, ok := result.Record().Values[0].(int64); ok {
				userIds = append(userIds, id)
			}
		}

		return nil, result.Err()
	})

	if err != nil {
		return nil, err
	}

	return &follower.UserListResponse{UserIds: userIds}, nil
}

func (s *FollowerServer) GetFollowers(ctx context.Context, req *follower.UserRequest) (*follower.UserListResponse, error) {
	log.Printf("GetFollowers for user: %d", req.UserId)

	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeRead})
	defer session.Close(ctx)

	userIds := []int64{}

	_, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		result, err := tx.Run(ctx,
			`MATCH (a:User)-[:FOLLOWS]->(b:User {id: $userId})
             RETURN a.id AS id`,
			map[string]any{
				"userId": req.UserId,
			})
		if err != nil {
			return nil, err
		}

		for result.Next(ctx) {
			if id, ok := result.Record().Values[0].(int64); ok {
				userIds = append(userIds, id)
			}
		}

		return nil, result.Err()
	})

	if err != nil {
		return nil, err
	}

	return &follower.UserListResponse{UserIds: userIds}, nil
}

func (s *FollowerServer) GetRecommendations(ctx context.Context, req *follower.UserRequest) (*follower.UserListResponse, error) {
	log.Printf("GetRecommendations for user: %d", req.UserId)

	session := s.Driver.NewSession(ctx, neo4j.SessionConfig{AccessMode: neo4j.AccessModeRead})
	defer session.Close(ctx)

	userIds := []int64{}

	_, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		result, err := tx.Run(ctx,
			`MATCH (me:User {id: $userId})-[:FOLLOWS]->(friend:User)-[:FOLLOWS]->(rec:User)
			 WHERE NOT (me)-[:FOLLOWS]->(rec) AND rec.id <> $userId
			 RETURN DISTINCT rec.id AS id
			 LIMIT 10`,
			map[string]any{"userId": req.UserId},
		)
		if err != nil {
			return nil, err
		}

		for result.Next(ctx) {
			if id, ok := result.Record().Values[0].(int64); ok {
				userIds = append(userIds, id)
			}
		}
		return nil, result.Err()
	})

	if err != nil {
		return nil, err
	}

	return &follower.UserListResponse{UserIds: userIds}, nil
}

