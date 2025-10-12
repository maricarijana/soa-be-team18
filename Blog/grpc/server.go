package grpcsrv

import (
	"context"
	"time"

	"soa/blog/model"
	"soa/blog/proto/blog"
	"soa/blog/service"

	"go.mongodb.org/mongo-driver/bson/primitive"
	"google.golang.org/protobuf/types/known/timestamppb"
)

type Server struct {
	blog.UnimplementedBlogServiceServer
	BlogSvc    *service.BlogService
	CommentSvc *service.CommentService
}

// --------- helpers: model -> proto ---------
func toProtoBlog(m *model.Blog) *blog.Blog {
	p := &blog.Blog{
		Id:          m.ID.Hex(),
		Title:       m.Title,
		Description: m.Description,
		ImageUrl:    m.ImageUrl,
		Status:      blog.BlogStatus(m.Status),
		UserId:      m.UserId,
		RatingSum:   int32(m.RatingSum),
	}
	// CreatedAt
	if !m.CreatedAt.IsZero() {
		p.CreatedAt = timestamppb.New(m.CreatedAt)
	} else {
		p.CreatedAt = timestamppb.New(time.Now())
	}
	// Ratings
	for _, r := range m.Ratings {
		p.Ratings = append(p.Ratings, &blog.Rating{
			UserId:    r.UserID,
			Value:     int32(r.Value),
			CreatedAt: timestamppb.New(r.CreatedAt),
		})
	}
	// Comments
	for _, c := range m.Comments {
		pc := &blog.Comment{
			Id:        c.ID.Hex(),
			BlogId:    c.BlogId.Hex(),
			Author:    c.Username,
			Text:      c.Text,
			CreatedAt: timestamppb.New(c.CreatedAt),
		}
		p.Comments = append(p.Comments, pc)
	}
	return p
}

// --------- RPC metode ---------

func (s *Server) CreateBlog(ctx context.Context, req *blog.CreateBlogRequest) (*blog.CreateBlogResponse, error) {
	m := &model.Blog{
		Title:       req.Title,
		Description: req.Description,
		ImageUrl:    req.ImageUrl,
		Status:      model.BlogStatus(req.Status),
		UserId:      req.UserId,
		CreatedAt:   time.Now(),
	}
	if err := s.BlogSvc.Create(m); err != nil {
		return nil, err
	}
	return &blog.CreateBlogResponse{Blog: toProtoBlog(m)}, nil
}

func (s *Server) LikeBlog(ctx context.Context, req *blog.LikeBlogRequest) (*blog.Blog, error) {
	objID, err := primitive.ObjectIDFromHex(req.BlogId) // konverzija string → ObjectID
	if err != nil {
		return nil, err
	}

	if err := s.BlogSvc.LikeBlog(objID, req.UserId); err != nil {
		return nil, err
	}

	updated, err := s.BlogSvc.BlogRepository.GetByID(objID)
	if err != nil {
		return nil, err
	}
	return toProtoBlog(updated), nil
}

func (s *Server) CreateComment(ctx context.Context, req *blog.CreateCommentRequest) (*blog.CreateCommentResponse, error) {
	objID, err := primitive.ObjectIDFromHex(req.BlogId)
	if err != nil {
		return nil, err
	}

	c := &model.Comment{
		BlogId:    objID,
		Username:  req.Author,
		Text:      req.Text,
		CreatedAt: time.Now(),
	}

	if err := s.CommentSvc.Create(c); err != nil {
		return nil, err
	}

	return &blog.CreateCommentResponse{
		Comment: &blog.Comment{
			Id:        c.ID.Hex(),
			BlogId:    c.BlogId.Hex(),
			Author:    c.Username,
			Text:      c.Text,
			CreatedAt: timestamppb.New(c.CreatedAt),
		},
	}, nil
}

func (s *Server) GetBlogs(ctx context.Context, req *blog.GetBlogsRequest) (*blog.GetBlogsResponse, error) {
	blogs, err := s.BlogSvc.BlogRepository.GetAll()
	if err != nil {
		return nil, err
	}

	var protoBlogs []*blog.Blog
	for _, m := range blogs {
		protoBlogs = append(protoBlogs, toProtoBlog(&m))
	}

	return &blog.GetBlogsResponse{Blogs: protoBlogs}, nil
}

func (s *Server) GetBlogById(ctx context.Context, req *blog.GetBlogByIdRequest) (*blog.GetBlogByIdResponse, error) {
	objID, err := primitive.ObjectIDFromHex(req.BlogId)
	if err != nil {
		return nil, err
	}

	// Pozivamo servisnu metodu koja dohvaća i blog i njegove komentare
	m, err := s.BlogSvc.GetByIDWithComments(objID)
	if err != nil {
		return nil, err
	}

	return &blog.GetBlogByIdResponse{Blog: toProtoBlog(m)}, nil
}
