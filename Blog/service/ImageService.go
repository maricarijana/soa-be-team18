package service

import (
	"encoding/base64"
	"fmt"
	"os"
	"path/filepath"
	"strings"
	"time"
)

type ImageService struct {
	BasePath string
}

func NewImageService(basePath string) *ImageService {
	return &ImageService{BasePath: basePath}
}

func (s *ImageService) SaveBase64Image(base64Data, folderName string) (string, error) {
	if base64Data == "" {
		return "", fmt.Errorf("empty image data")
	}

	// ✅ Odredi ekstenziju iz prefiksa
	var ext string
	switch {
	case strings.HasPrefix(base64Data, "data:image/png"):
		ext = ".png"
	case strings.HasPrefix(base64Data, "data:image/jpeg"):
		ext = ".jpg"
	case strings.HasPrefix(base64Data, "data:image/jpg"):
		ext = ".jpg"
	case strings.HasPrefix(base64Data, "data:image/gif"):
		ext = ".gif"
	case strings.HasPrefix(base64Data, "data:image/webp"):
		ext = ".webp"
	default:
		return "", fmt.Errorf("unsupported image format")
	}

	// ✅ Ukloni "data:image/...;base64," deo
	parts := strings.Split(base64Data, ",")
	if len(parts) != 2 {
		return "", fmt.Errorf("invalid base64 format")
	}
	imageData, err := base64.StdEncoding.DecodeString(parts[1])
	if err != nil {
		return "", fmt.Errorf("failed to decode base64: %v", err)
	}

	// ✅ Kreiraj direktorijum ako ne postoji
	dir := filepath.Join(s.BasePath, "images", folderName)
	if _, err := os.Stat(dir); os.IsNotExist(err) {
		os.MkdirAll(dir, os.ModePerm)
	}

	// ✅ Snimi fajl sa pravom ekstenzijom
	fileName := fmt.Sprintf("%d%s", time.Now().UnixNano(), ext)
	filePath := filepath.Join(dir, fileName)
	err = os.WriteFile(filePath, imageData, 0644)
	if err != nil {
		return "", fmt.Errorf("failed to write image: %v", err)
	}

	// ✅ Vrati relativnu putanju (frontend koristi ovu)
	return fmt.Sprintf("images/%s/%s", folderName, fileName), nil
}

