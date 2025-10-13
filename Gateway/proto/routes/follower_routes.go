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

func proxyRequest(w http.ResponseWriter, r *http.Request, url string) {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		http.Error(w, "Failed to read request body", http.StatusBadRequest)
		return
	}
	// vrati r.Body ako će se još negde čitati (opciono)
	r.Body = io.NopCloser(bytes.NewBuffer(body))

	req, err := http.NewRequest(http.MethodPost, url, bytes.NewReader(body))
	if err != nil {
		http.Error(w, "Failed to create request", http.StatusInternalServerError)
		return
	}

	// Kopiraj SVA zaglavlja, posebno Authorization
	req.Header = r.Header.Clone()
	req.Header.Set("Content-Type", "application/json")

	resp, err := http.DefaultClient.Do(req)
	if err != nil {
		log.Printf("Error forwarding request to %s: %v", url, err)
		http.Error(w, "Failed to reach follower service", http.StatusBadGateway)
		return
	}
	defer resp.Body.Close()

	// Propagiraj response headers (npr. CORS, content-type…)
	for k, vv := range resp.Header {
		for _, v := range vv {
			w.Header().Add(k, v)
		}
	}

	w.WriteHeader(resp.StatusCode)
	_, _ = io.Copy(w, resp.Body)
}

func proxyGetRequest(w http.ResponseWriter, r *http.Request, url string) {
	req, err := http.NewRequest(http.MethodGet, url, nil)
	if err != nil {
		http.Error(w, "Failed to create request", http.StatusInternalServerError)
		return
	}
	req.Header = r.Header.Clone()

	resp, err := http.DefaultClient.Do(req)
	if err != nil {
		log.Printf("Error forwarding GET to %s: %v", url, err)
		http.Error(w, "Failed to reach follower service", http.StatusBadGateway)
		return
	}
	defer resp.Body.Close()

	// 👇 ovo dodaj
	bodyBytes, _ := io.ReadAll(resp.Body)
	log.Printf("📡 Response from %s: %s", url, string(bodyBytes))

	// vrati telo na front
	for k, vv := range resp.Header {
		for _, v := range vv {
			w.Header().Add(k, v)
		}
	}
	w.WriteHeader(resp.StatusCode)
	w.Write(bodyBytes)
}
