FROM golang:1.24.5-alpine AS blogs-builder
WORKDIR /app

COPY go.mod .
COPY go.sum .
RUN go mod download

COPY . .
RUN go build -o blog-app

FROM alpine

RUN mkdir -p /app/wwwroot/images/blogs

WORKDIR /app

COPY --from=blogs-builder /app/blog-app /usr/bin/blog-app
COPY --from=blogs-builder /app/wwwroot /app/wwwroot
EXPOSE 9090
ENTRYPOINT ["/usr/bin/blog-app"]