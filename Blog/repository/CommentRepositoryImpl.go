package repository

import (
	"context"
	"soa/blog/model"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
)

type CommentRepositoryImpl struct {
	Collection *mongo.Collection
}

func (r *CommentRepositoryImpl) Create(comment *model.Comment) error {
	_, err := r.Collection.InsertOne(context.TODO(), comment)
	return err
}

func (r *CommentRepositoryImpl) GetAll() ([]model.Comment, error) {
	cur, err := r.Collection.Find(context.TODO(), bson.D{})
	if err != nil {
		return nil, err
	}
	defer cur.Close(context.TODO())

	var comments []model.Comment
	if err := cur.All(context.TODO(), &comments); err != nil {
		return nil, err
	}
	return comments, nil
}

func (r *CommentRepositoryImpl) GetByID(id primitive.ObjectID) (*model.Comment, error) {
	filter := bson.D{{Key: "_id", Value: id}}
	var comment model.Comment
	err := r.Collection.FindOne(context.TODO(), filter).Decode(&comment)
	if err != nil {
		return nil, err
	}
	return &comment, nil
}

func (r *CommentRepositoryImpl) GetByBlogID(blogId primitive.ObjectID) ([]model.Comment, error) {
	filter := bson.D{{Key: "blogId", Value: blogId}}
	cur, err := r.Collection.Find(context.TODO(), filter)
	if err != nil {
		return nil, err
	}
	defer cur.Close(context.TODO())

	var comments []model.Comment
	if err := cur.All(context.TODO(), &comments); err != nil {
		return nil, err
	}
	return comments, nil
}

func (r *CommentRepositoryImpl) Update(comment *model.Comment) error {
	filter := bson.D{{Key: "_id", Value: comment.ID}}
	update := bson.D{{Key: "$set", Value: comment}}
	_, err := r.Collection.UpdateOne(context.TODO(), filter, update)
	return err
}

func (r *CommentRepositoryImpl) Delete(id primitive.ObjectID) error {
	filter := bson.D{{Key: "_id", Value: id}}
	_, err := r.Collection.DeleteOne(context.TODO(), filter)
	return err
}
