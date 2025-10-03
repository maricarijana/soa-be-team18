package main

import (
	"context"
	"time"

	// "gateway/proto/stakeholders"
	"log"
	"net"
	"net/http"
	"soa/blog/proto/blog"

	stakeholders "gateway/proto/stakeholders"
	tours "gateway/proto/tours"

	// stakeholders "soa/blog/proto/stakeholders"

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

	// connStakeholders, err := grpc.DialContext(
	//     context.Background(),
	//     "stakeholders-service:80",
	//     grpc.WithTransportCredentials(insecure.NewCredentials()),
	//     grpc.WithBlock(),
	// )
	// if err != nil {
	//     log.Fatalln("Failed to dial stakeholders-service:", err)
	// }

	// --- 3. REST gateway mux ---
	gwmux := runtime.NewServeMux()

	// Registruj Blog servis REST handler
	err = blog.RegisterBlogServiceHandler(context.Background(), gwmux, conn)
	if err != nil {
		log.Fatalln("Failed to register blog gateway:", err)
	}

	// if err := stakeholders.RegisterStakeholdersServiceHandler(context.Background(), gwmux, connStakeholders); err != nil {
	//     log.Fatalln("Failed to register stakeholders gateway:", err)
	// }
	// log.Println("Dialing stakeholders-service...")
	// connStakeholders, err := grpc.DialContext(
	// 	context.Background(),
	// 	"stakeholders-service:80",
	// 	grpc.WithTransportCredentials(insecure.NewCredentials()),
	// 	grpc.WithBlock(),
	// )
	// if err != nil {
	// 	log.Fatalln("Failed to dial stakeholders-service:", err)
	// }
	// log.Println("Dial OK, registering handler...")

	// if err := stakeholders.RegisterStakeholdersServiceHandler(context.Background(), gwmux, connStakeholders); err != nil {
	// 	log.Fatalln("Failed to register stakeholders gateway:", err)
	// }
	// log.Println("Stakeholders handler registered successfully")
	log.Println("Dialing stakeholders-service...")

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	connStakeholders, err := grpc.DialContext(
		ctx,
		"stakeholders-service:80",
		grpc.WithTransportCredentials(insecure.NewCredentials()),
		grpc.WithBlock(),
	)
	if err != nil {
		log.Printf("Failed to dial stakeholders-service: %v", err)
	} else {
		log.Println("Dial OK, registering handler...")
		if err := stakeholders.RegisterStakeholdersServiceHandler(context.Background(), gwmux, connStakeholders); err != nil {
			log.Printf("Failed to register stakeholders gateway: %v", err)
		} else {
			log.Println("Stakeholders handler registered successfully")
		}
	}

	// log.Println("Dial OK, registering handler...")

	// if err := stakeholders.RegisterStakeholdersServiceHandler(context.Background(), gwmux, connStakeholders); err != nil {
	// 	log.Fatalln("Failed to register stakeholders gateway:", err)
	// }

	// 👉 ovde kasnije možeš dodati i druge servise:
	// stakeholders.RegisterStakeholderServiceHandler(ctx, gwmux, conn2)
	// orders.RegisterOrderServiceHandler(ctx, gwmux, conn3)
	// itd.

	//TOURS SERVICE

	connTours, err := grpc.DialContext(
		ctx,
		"tours-service:90", // ime servisa iz docker-compose i port na kojem sluša
		grpc.WithTransportCredentials(insecure.NewCredentials()),
		grpc.WithBlock(),
	)
	if err != nil {
		log.Printf("Failed to dial tours-service: %v", err)
	} else {
		log.Println("Dial OK, registering handler for tours...")
		if err := tours.RegisterToursServiceHandler(context.Background(), gwmux, connTours); err != nil {
			log.Printf("Failed to register tours gateway: %v", err)
		} else {
			log.Println("Tours handler registered successfully")
		}
		
		if err := tours.RegisterKeyPointServiceHandler(context.Background(), gwmux, connTours); err != nil {
        log.Printf("Failed to register keypoints gateway: %v", err)
    	} else {
        log.Println("KeyPoints handler registered successfully")
    	}
	}

	// --- 4. Start REST server ---
	gwServer := &http.Server{
		Addr:    ":8090", // REST ulazni port
		Handler: gwmux,
	}

	log.Println("Serving gRPC-Gateway on http://0.0.0.0:8090")
	log.Fatalln(gwServer.ListenAndServe())
}
