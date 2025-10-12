using Grpc.Core;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using Tours.Core.UseCases;
//using Tours.Core.UseCases.Author;
using GrpcServiceTranscoding;
using Tours.Application.Public;

namespace Tours.API.Controllers
{
    public class KeyPointProtoController : KeyPointService.KeyPointServiceBase
    {
        private readonly ILogger<KeyPointProtoController> _logger;
        private readonly IKeyPointService _keyPointService;
        private readonly IWebHostEnvironment _env;
        private readonly IImageService _imageService;

        public KeyPointProtoController(
            ILogger<KeyPointProtoController> logger,
            IKeyPointService keyPointService,
            IWebHostEnvironment env,
            IImageService imageService)
        {
            _logger = logger;
            _keyPointService = keyPointService;
            _env = env;
            _imageService = imageService;
        }

        public override Task<KeyPoint> AddKeyPoint(AddKeyPointRequest request, ServerCallContext context)
        {
            _logger.LogInformation("AddKeyPoint called for {Name}", request.Name);

            var dto = new KeyPointDto
            {
                Name = request.Name,
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                Description = request.Description,
                ImageBase64 = request.ImageBase64,
                TourId = request.TourId,
                PublicStatus = (PublicStatus)request.PublicStatus
            };

            if (!string.IsNullOrEmpty(dto.ImageBase64))
            {
                var imgBytes = Convert.FromBase64String(dto.ImageBase64.Split(',').Last());
                var folder = Path.Combine(_env.WebRootPath, "images", "keypoints");
                dto.Image = _imageService.SaveImage(folder, imgBytes, "keypoints");
            }

            var result = _keyPointService.Create(dto);

            if (!result.IsSuccess || result.Value == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, result.Errors.FirstOrDefault()?.Message ?? "Failed to add keypoint"));

            var kp = result.Value;
            return Task.FromResult(new KeyPoint
            {
                Id = kp.Id,
                Name = kp.Name,
                Longitude = kp.Longitude,
                Latitude = kp.Latitude,
                Description = kp.Description,
                Image = kp.Image,
                TourId = kp.TourId,
                PublicStatus = (int)kp.PublicStatus
            });
        }


        public override Task<KeyPoint> UpdateKeyPoint(KeyPoint request, ServerCallContext context)
        {
            _logger.LogInformation("UpdateKeyPoint called for id {Id}", request.Id);

            var dto = new KeyPointDto
            {
                Id = request.Id,
                Name = request.Name,
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                Description = request.Description,
                Image = request.Image,
                ImageBase64 = request.ImageBase64,
                TourId = request.TourId,
                PublicStatus = (PublicStatus)request.PublicStatus
            };

            if (!string.IsNullOrEmpty(dto.ImageBase64))
            {
                if (!string.IsNullOrEmpty(dto.Image))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, dto.Image);
                    _imageService.DeleteOldImage(oldPath);
                }
                var imgBytes = Convert.FromBase64String(dto.ImageBase64.Split(',').Last());
                var folder = Path.Combine(_env.WebRootPath, "images", "keypoints");
                dto.Image = _imageService.SaveImage(folder, imgBytes, "keypoints");
            }

            var result = _keyPointService.Update(dto);

            if (!result.IsSuccess || result.Value == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, result.Errors.FirstOrDefault()?.Message ?? "Failed to update keypoint"));

            var kp = result.Value;
            return Task.FromResult(new KeyPoint
            {
                Id = kp.Id,
                Name = kp.Name,
                Longitude = kp.Longitude,
                Latitude = kp.Latitude,
                Description = kp.Description,
                Image = kp.Image,
                TourId = kp.TourId,
                PublicStatus = (int)kp.PublicStatus
            });
        }

        public override Task<DeleteKeyPointResponse> DeleteKeyPoint(DeleteKeyPointRequest request, ServerCallContext context)
        {
            _logger.LogInformation("DeleteKeyPoint called for id {Id}", request.Id);

            var result = _keyPointService.Delete((int)request.Id);
            return Task.FromResult(new DeleteKeyPointResponse { Success = result.IsSuccess });
        }

        public override Task<GetKeyPointsByUserResponse> GetKeyPointsByUser(GetKeyPointsByUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetKeyPointsByUser called for user {UserId}", request.UserId);

            var result = _keyPointService.GetByUserId(request.UserId);

            if (!result.IsSuccess || result.Value == null)
                throw new RpcException(new Status(StatusCode.NotFound, result.Errors.FirstOrDefault()?.Message ?? "No keypoints found"));

            var resp = new GetKeyPointsByUserResponse();
            resp.KeyPoints.AddRange(result.Value.Select(kp => new KeyPoint
            {
                Id = kp.Id,
                Name = kp.Name,
                Longitude = kp.Longitude,
                Latitude = kp.Latitude,
                Description = kp.Description,
                Image = kp.Image,
                TourId = kp.TourId,
                PublicStatus = (int)kp.PublicStatus
            }));

            return Task.FromResult(resp);
        }

        public override Task<KeyPoint> GetKeyPointById(GetKeyPointByIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetKeyPointById called for id {Id}", request.Id);

            var result = _keyPointService.Get((int)request.Id);

            if (!result.IsSuccess || result.Value == null)
                throw new RpcException(new Status(StatusCode.NotFound, result.Errors.FirstOrDefault()?.Message ?? "Keypoint not found"));

            var kp = result.Value;
            return Task.FromResult(new KeyPoint
            {
                Id = kp.Id,
                Name = kp.Name,
                Longitude = kp.Longitude,
                Latitude = kp.Latitude,
                Description = kp.Description,
                Image = kp.Image,
                TourId = kp.TourId,
                PublicStatus = (int)kp.PublicStatus
            });
        }
        public override Task<GetKeyPointsByTourResponse> GetKeyPointsByTour(GetKeyPointsByTourRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetKeyPointsByTour called for tour {TourId}", request.TourId);

            var result = _keyPointService.GetByTourId(request.TourId);

            if (!result.IsSuccess || result.Value == null)
                throw new RpcException(new Status(StatusCode.NotFound, result.Errors.FirstOrDefault()?.Message ?? "No keypoints found"));

            var resp = new GetKeyPointsByTourResponse();
            resp.KeyPoints.AddRange(result.Value.Select(kp => new KeyPoint
            {
                Id = kp.Id,
                Name = kp.Name,
                Longitude = kp.Longitude,
                Latitude = kp.Latitude,
                Description = kp.Description,
                Image = kp.Image,
                TourId = kp.TourId,
                PublicStatus = (int)kp.PublicStatus
            }));

            return Task.FromResult(resp);
        }

    }
}
