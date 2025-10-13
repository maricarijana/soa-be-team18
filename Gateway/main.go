package main

import (
	"context"
	"io"
	"time"

	// "gateway/proto/stakeholders"
	"gateway/proto/routes"
	stakeholders "gateway/proto/stakeholders"
	"log"
	"net"
	"net/http"
	"soa/blog/proto/blog"

	tours "gateway/proto/tours"

	// stakeholders "soa/blog/proto/stakeholders"

	"github.com/grpc-ecosystem/grpc-gateway/v2/runtime"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

// CORS middleware
func withCORS(h http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Access-Control-Allow-Origin", "http://localhost:4200") // Angular app
		w.Header().Set("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
		w.Header().Set("Access-Control-Allow-Headers", "Content-Type, Authorization")

		if r.Method == http.MethodOptions {
			w.WriteHeader(http.StatusNoContent)
			return
		}

		h.ServeHTTP(w, r)
	})
}
func main() {
	// --- 1. Startujemo gRPC server ---
	grpcLis, err := net.Listen("tcp", ":8080")
	if err != nil {
		log.Fatalln("Failed to listen:", err)
	}

	grpcServer := grpc.NewServer()

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
	
	rootMux := http.NewServeMux()

	gwmux.HandlePath("GET", "/images/{path=**}", func(w http.ResponseWriter, r *http.Request, _ map[string]string) {
    target := "http://blog-service:8080" + r.URL.Path
    resp, err := http.Get(target)
    if err != nil {
        http.Error(w, "Error fetching image from blog-service", http.StatusBadGateway)
        return
    }
    defer resp.Body.Close()

    w.Header().Set("Content-Type", resp.Header.Get("Content-Type"))
    w.WriteHeader(resp.StatusCode)
    _, _ = io.Copy(w, resp.Body)
})

	// Registruj Blog servis REST handler
	err = blog.RegisterBlogServiceHandler(context.Background(), gwmux, conn)
	if err != nil {
		log.Fatalln("Failed to register blog gateway:", err)
	}

	log.Println("Dialing stakeholders-service...")

	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
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

	// --- FOLLOWER SERVICE (REST proxy) ---
	routes.RegisterFollowerRoutes(gwmux)

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

		if err := tours.RegisterTourReviewServiceHandler(context.Background(), gwmux, connTours); err != nil {
			log.Printf("Failed to register tour reviews gateway: %v", err)
		} else {
			log.Println("TourReviews handler registered successfully")
		}
		if err := tours.RegisterPositionSimulatorServiceHandler(context.Background(), gwmux, connTours); err != nil {
			log.Printf("Failed to register PositionSimulator gateway: %v", err)
		} else {
			log.Println("PositionSimulator handler registered successfully")
		}

		if err := tours.RegisterShoppingCartServiceHandler(context.Background(), gwmux, connTours); err != nil {
    log.Printf("Failed to register ShoppingCart gateway: %v", err)
} else {
    log.Println("ShoppingCart handler registered successfully")
}

	}
	// proxy REST zahteva ka TourExecutionController
proxyToTours := func(w http.ResponseWriter, r *http.Request) {
	log.Printf("[Gateway → Tours] %s %s", r.Method, r.URL.Path)

	targetURL := "http://tours-service:5000" + r.URL.Path  // <--- promenjeno
	if r.URL.RawQuery != "" {
		targetURL += "?" + r.URL.RawQuery
	}

	req, err := http.NewRequest(r.Method, targetURL, r.Body)
	if err != nil {
		http.Error(w, "Failed to create request: "+err.Error(), http.StatusInternalServerError)
		return
	}
	req.Header = r.Header.Clone()

	client := &http.Client{Timeout: 5 * time.Second}
	resp, err := client.Do(req)
	if err != nil {
		http.Error(w, "Error contacting tours-service: "+err.Error(), http.StatusBadGateway)
		return
	}
	defer resp.Body.Close()

	for k, v := range resp.Header {
		for _, vv := range v {
			w.Header().Add(k, vv)
		}
	}
	w.WriteHeader(resp.StatusCode)
	_, _ = io.Copy(w, resp.Body)
}

// pokrivamo i /api/tour-execution i /api/tour-execution/
rootMux.HandleFunc("/api/tour-execution", proxyToTours)
rootMux.HandleFunc("/api/tour-execution/", proxyToTours)

	
	rootMux.Handle("/", gwmux)


	// --- 4. Start REST server ---
	gwServer := &http.Server{
    Addr:    ":8090",
    Handler: withCORS(rootMux),
}


	log.Println("Serving gRPC-Gateway on http://0.0.0.0:8090")
	log.Fatalln(gwServer.ListenAndServe())
}
