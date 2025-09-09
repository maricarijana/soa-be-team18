package main

import (
	"context"
	"log"
	"net"
	"net/http"
	"soa/blog/proto/blog"

	"github.com/grpc-ecosystem/grpc-gateway/v2/runtime"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

func main() {
	// --- 1. Startujemo gRPC server ---
	grpcLis, err := net.Listen("tcp", ":8080")
	if err != nil {
		log.Fatalln("Failed to listen:", err)
	}

	grpcServer := grpc.NewServer()

	// Ovde bi registrovao prave implementacije servisa
	// ali gateway uglavnom ne implementira logiku,
	// nego samo prosljeđuje, pa ovo možeš preskočiti
	// ako gRPC server za Blog već radi u posebnom kontejneru.
	// blog.RegisterBlogServiceServer(grpcServer, &YourBlogServerImpl{})

	log.Println("Serving gRPC on 0.0.0.0:8080")
	go func() {
		log.Fatalln(grpcServer.Serve(grpcLis))
	}()

	// --- 2. Kreiramo gRPC konekciju (client) ---
	conn, err := grpc.DialContext(
		context.Background(),
		"blog-service:9090", 
		grpc.WithTransportCredentials(insecure.NewCredentials()),
		grpc.WithBlock(),
	)
	if err != nil {
		log.Fatalln("Failed to dial blog-service:", err)
	}

	// --- 3. REST gateway mux ---
	gwmux := runtime.NewServeMux()

	// Registruj Blog servis REST handler
	err = blog.RegisterBlogServiceHandler(context.Background(), gwmux, conn)
	if err != nil {
		log.Fatalln("Failed to register blog gateway:", err)
	}

	// 👉 ovde kasnije možeš dodati i druge servise:
	// stakeholders.RegisterStakeholderServiceHandler(ctx, gwmux, conn2)
	// orders.RegisterOrderServiceHandler(ctx, gwmux, conn3)
	// itd.

	// --- 4. Start REST server ---
	gwServer := &http.Server{
		Addr:    ":8090", // REST ulazni port
		Handler: gwmux,
	}

	log.Println("Serving gRPC-Gateway on http://0.0.0.0:8090")
	log.Fatalln(gwServer.ListenAndServe())
}
