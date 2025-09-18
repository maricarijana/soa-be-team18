# 1. build stage
FROM golang:1.24.5-alpine AS builder
WORKDIR /app

# Kopiraj go mod i go sum fajlove
COPY go.mod go.sum ./
RUN go mod download

# Kopiraj ceo Follower servis
COPY . .

# Build binarni fajl
RUN go build -o follower-app .

# 2. runtime stage
FROM alpine:latest
WORKDIR /root/

COPY --from=builder /app/follower-app .

EXPOSE 9091

CMD ["./follower-app"]
