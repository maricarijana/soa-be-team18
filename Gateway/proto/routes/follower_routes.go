package routes

import (
	"bytes"
	"io"
	"log"
	"net/http"

	"github.com/grpc-ecosystem/grpc-gateway/v2/runtime"
)

// RegisterFollowerRoutes registruje REST proxy rute za follower servis
func RegisterFollowerRoutes(mux *runtime.ServeMux) {

	mux.HandlePath("POST", "/api/follow", func(w http.ResponseWriter, r *http.Request, pathParams map[string]string) {
		proxyRequest(w, r, "http://follower-service:9091/follow")
	})

	mux.HandlePath("POST", "/api/unfollow", func(w http.ResponseWriter, r *http.Request, pathParams map[string]string) {
		proxyRequest(w, r, "http://follower-service:9091/unfollow")
	})

	mux.HandlePath("GET", "/api/followers/{id}", func(w http.ResponseWriter, r *http.Request, pathParams map[string]string) {
		id := pathParams["id"]
		proxyGetRequest(w, r, "http://follower-service:9091/followers/"+id)
	})

	mux.HandlePath("GET", "/api/following/{id}", func(w http.ResponseWriter, r *http.Request, pathParams map[string]string) {
		id := pathParams["id"]
		proxyGetRequest(w, r, "http://follower-service:9091/following/"+id)
	})

	mux.HandlePath("GET", "/api/recommendations/{id}", func(w http.ResponseWriter, r *http.Request, pathParams map[string]string) {
		id := pathParams["id"]
		proxyGetRequest(w, r, "http://follower-service:9091/recommendations/"+id)
	})
}

// proxyRequest šalje POST zahteve follower-servisu
func proxyRequest(w http.ResponseWriter, r *http.Request, url string) {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		http.Error(w, "Failed to read request body", http.StatusBadRequest)
		return
	}

	resp, err := http.Post(url, "application/json", bytes.NewBuffer(body))
	if err != nil {
		log.Printf("Error forwarding request to %s: %v", url, err)
		http.Error(w, "Failed to reach follower service", http.StatusBadGateway)
		return
	}
	defer resp.Body.Close()

	w.WriteHeader(resp.StatusCode)
	io.Copy(w, resp.Body)
}

// proxyGetRequest šalje GET zahteve follower-servisu
func proxyGetRequest(w http.ResponseWriter, r *http.Request, url string) {
	resp, err := http.Get(url)
	if err != nil {
		log.Printf("Error forwarding GET to %s: %v", url, err)
		http.Error(w, "Failed to reach follower service", http.StatusBadGateway)
		return
	}
	defer resp.Body.Close()

	w.WriteHeader(resp.StatusCode)
	io.Copy(w, resp.Body)
}
