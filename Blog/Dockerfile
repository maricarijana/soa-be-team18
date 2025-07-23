FROM golang:1.24.5-alpine AS blogs-builder
WORKDIR /app

COPY go.mod .
COPY go.sum .
RUN go mod download

COPY . .
RUN go build -o blog-app

FROM alpine
COPY --from=blogs-builder /app/blog-app /usr/bin/blog-app
EXPOSE 8082
ENTRYPOINT ["/usr/bin/blog-app"]