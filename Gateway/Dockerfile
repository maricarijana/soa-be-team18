# 1. Stage: build
FROM golang:1.24.5-alpine AS builder
WORKDIR /app

# Kopiraj samo mod fajlove da se kešira go mod download
COPY Gateway/go.mod Gateway/go.sum ./

# Kopiraj ceo Blog modul i Gateway kod
COPY Blog ./Blog
COPY Gateway ./Gateway

# Prebaci se u Gateway folder
WORKDIR /app/Gateway

# Povuci dependencije
RUN go mod download

# Build binarni fajl
RUN CGO_ENABLED=0 GOOS=linux go build -o gateway-app .

# 2. Stage: runtime
FROM alpine:latest
WORKDIR /root/

# Kopiraj samo binary iz builder stage-a
COPY --from=builder /app/Gateway/gateway-app .

EXPOSE 8090
CMD ["./gateway-app"]
