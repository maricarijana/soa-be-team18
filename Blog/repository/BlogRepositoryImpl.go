package repository

import (
	"context"
	"soa/blog/model"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
)

type BlogRepositoryImpl struct {
	Collection *mongo.Collection
}

func (r *BlogRepositoryImpl) Create(blog *model.Blog) error {
	_, err := r.Collection.InsertOne(context.TODO(), blog)
	return err
}

func (r *BlogRepositoryImpl) GetByID(id primitive.ObjectID) (*model.Blog, error) {
	filter := bson.D{{Key: "_id", Value: id}}
	var blog model.Blog
	err := r.Collection.FindOne(context.TODO(), filter).Decode(&blog)
	if err != nil {
		return nil, err
	}
	return &blog, nil
}

func (r *BlogRepositoryImpl) GetAll() ([]model.Blog, error) {
	cur, err := r.Collection.Find(context.TODO(), bson.D{})
	if err != nil {
		return nil, err
	}
	defer cur.Close(context.TODO())

	var blogs []model.Blog
	if err := cur.All(context.TODO(), &blogs); err != nil {
		return nil, err
	}
	return blogs, nil
}

func (r *BlogRepositoryImpl) Update(blog *model.Blog) error {
	filter := bson.D{{Key: "_id", Value: blog.ID}}
	update := bson.D{{Key: "$set", Value: blog}}
	_, err := r.Collection.UpdateOne(context.TODO(), filter, update)
	return err
}

func (r *BlogRepositoryImpl) Delete(id primitive.ObjectID) error {
	_, err := r.Collection.DeleteOne(context.TODO(), bson.M{"_id": id})
	return err
}
