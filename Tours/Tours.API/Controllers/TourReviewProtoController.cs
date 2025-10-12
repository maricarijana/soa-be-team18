using Grpc.Core;
using GrpcServiceTranscoding;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Tours.Application.Dtos;
using Tours.Application.Public;
using Tours.Application.Public.Author;
using Tours.Core.UseCases;

namespace Tours.API.Controllers
{
    public class TourReviewProtoController : TourReviewService.TourReviewServiceBase
    {
        private readonly ILogger<TourReviewProtoController> _logger;
        private readonly ITourReviewService _reviewService;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _env;

        public TourReviewProtoController(
            ILogger<TourReviewProtoController> logger,
            ITourReviewService reviewService,
            IImageService imageService,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _reviewService = reviewService;
            _imageService = imageService;
            _env = env;
        }

        public override Task<TourReview> AddTourReview(AddTourReviewRequest request, ServerCallContext context)
        {
            _logger.LogInformation("AddTourReview called for Tour {TourId} by User {UserId}",
                request.IdTour, request.IdTourist);

            var dto = new TourReviewDto
            {
                IdTour = request.IdTour,
                IdTourist = request.IdTourist,
                Rating = request.Rating,
                Comment = request.Comment,
                DateTour = !string.IsNullOrEmpty(request.DateTour)
                ? DateTime.SpecifyKind(DateTime.Parse(request.DateTour, null, DateTimeStyles.AdjustToUniversal), DateTimeKind.Utc)
                : DateTime.UtcNow,

                    DateComment = !string.IsNullOrEmpty(request.DateComment)
                ? DateTime.SpecifyKind(DateTime.Parse(request.DateComment, null, DateTimeStyles.AdjustToUniversal), DateTimeKind.Utc)
                : DateTime.UtcNow,
            };

            _logger.LogInformation("Received {Count} images in request", request.ImagesBase64.Count);
            foreach (var base64 in request.ImagesBase64)
            {
                try
                {
                    var imgBytes = Convert.FromBase64String(base64.Split(',').Last());
                    var folder = Path.Combine(_env.WebRootPath, "images", "reviews");
                    var savedPath = _imageService.SaveImage(folder, imgBytes, "reviews");

                    dto.Images.Add(savedPath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Invalid image skipped: {Message}", ex.Message);
                }
            }

            var result = _reviewService.Create(dto);
            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    result.Errors.FirstOrDefault()?.Message ?? "Failed to add review"));
            }

            var review = result.Value;
            return Task.FromResult(new TourReview
            {
                Id = review.Id,
                IdTour = review.IdTour,
                IdTourist = review.IdTourist,
                Rating = review.Rating,
                Comment = review.Comment ?? "",
                DateTour = review.DateTour?.ToString("o") ?? "",
                DateComment = review.DateComment?.ToString("o") ?? "",
                Images = { review.Images }
            });
        }
    }
}
