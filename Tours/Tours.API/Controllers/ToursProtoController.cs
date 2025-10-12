using Grpc.Core;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using System.Net;
using GrpcServiceTranscoding;
using Google.Protobuf.WellKnownTypes;

namespace Tours.API.Controllers
{
    public class ToursProtoController : ToursService.ToursServiceBase
    {
        private readonly ILogger<ToursProtoController> _logger;
        private readonly ITourService _tourService;

        public ToursProtoController(
            ILogger<ToursProtoController> logger,
            ITourService tourService)
        {
            _logger = logger;
            _tourService = tourService;
        }

        public override Task<GrpcServiceTranscoding.Tour> AddTour(GrpcServiceTranscoding.AddTourRequest request, ServerCallContext context)
        {
            _logger.LogInformation("AddTour called for tour name {Name}", request.Name);

            var dto = new TourDto
            {
                //Name = request.Name,
                //Description = request.Description,
                //Difficulty = request.Difficulty,
                //Tags = null,
                //Status = 0,
                //Price = 0,
                //UserId = request.UserId,
                //LengthInKm = 0,
                //PublishedTime = DateTime.Now,
                //ArchiveTime = DateTime.Now,
                Name = request.Name,
                Description = request.Description,
                Difficulty = request.Difficulty,
                Tags = request.Tags
    .Select(t => System.Enum.TryParse<TourTags>(t, true, out var tag) ? tag : default)
    .ToList(),

                Status = (TourStatus)(int.TryParse(request.Status, out var status) ? status : 0), // ako status u requestu šalješ kao string
                Price = request.Price,
                UserId = request.UserId,
                LengthInKm = request.LengthInKm,
                PublishedTime = !string.IsNullOrEmpty(request.PublishedTime)
                        ? DateTime.SpecifyKind(DateTime.Parse(request.PublishedTime), DateTimeKind.Utc)
                        : DateTime.UtcNow,
                ArchiveTime = !string.IsNullOrEmpty(request.ArchiveTime)
                        ? DateTime.SpecifyKind(DateTime.Parse(request.ArchiveTime), DateTimeKind.Utc)
                        : DateTime.UtcNow
            };

            var result = _tourService.Create(dto);

            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, result.Errors.FirstOrDefault()?.Message ?? "Failed to add tour"));
            }

            return Task.FromResult(new Tour
            {
                //Id = result.Value.Id,
                //Name = result.Value.Name,
                //Description = result.Value.Description,
                //Difficulty = result.Value.Difficulty,
                //Tags = {  },
                //Status = " ",
                //Price = (double)result.Value.Price,
                //UserId = result.Value.UserId,
                //LengthInKm = (double)result.Value.LengthInKm,
                //PublishedTime = " ",
                //ArchiveTime = " "
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                Difficulty = result.Value.Difficulty,
                Tags = { result.Value.Tags?.Select(t => t.ToString()) ?? new List<string>() },

                Status = result.Value.Status.ToString(),
                Price = (double)result.Value.Price,
                UserId = result.Value.UserId,
                LengthInKm = (double)result.Value.LengthInKm,
                PublishedTime = result.Value.PublishedTime.ToString("o"), // ISO 8601 format
                ArchiveTime = result.Value.ArchiveTime?.ToString("o")
            });
        }

        public override Task<GetToursByUserResponse> GetToursByUser(GetToursByUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetToursByUser called for user {UserId}", request.UserId);

            var result = _tourService.GetByUserId(request.UserId);

            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, result.Errors.FirstOrDefault()?.Message ?? "No tours found for this user"));
            }

            var response = new GetToursByUserResponse();
            response.Tours.AddRange(result.Value.Select(t => new GrpcServiceTranscoding.Tour
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Difficulty = t.Difficulty,
                Tags = { t.Tags.Select(tag => tag.ToString()) },
                Status = t.Status.ToString(),
                Price = t.Price,
                UserId = t.UserId,
                LengthInKm = t.LengthInKm,
                PublishedTime = t.PublishedTime.ToString("o"),
                ArchiveTime = t.ArchiveTime?.ToString("o") ?? ""
            }));

            return Task.FromResult(response);
        }


        public override Task<Tour> GetTourById(GetTourByIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetTourById called for ID {Id}", request.Id);

            var result = _tourService.GetTourById(request.Id);

            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Tour not found"));
            }

            var t = result.Value;

            var response = new Tour
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Difficulty = t.Difficulty,
                Tags = { t.Tags.Select(tag => tag.ToString()) },
                Status = t.Status.ToString(),
                Price = (double)t.Price,
                UserId = t.UserId,
                LengthInKm = (double)t.LengthInKm,
                PublishedTime = t.PublishedTime.ToString("o"),
                ArchiveTime = t.ArchiveTime?.ToString("o") ?? ""
            };

            return Task.FromResult(response);
        }

        public override Task<Google.Protobuf.WellKnownTypes.Empty> AddTourDuration(
        AddTourDurationRequest request,
        ServerCallContext context)
        {
            _logger.LogInformation("AddTourDuration called for TourId {TourId}", request.TourId);

            // mapiranje TransportType iz requesta (string -> enum)
            if (!System.Enum.TryParse<TransportType>(request.Transport, true, out var transport))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Invalid transport type: {request.Transport}"));
            }

           
            var result = _tourService.AddDuration(request.TourId, transport, request.DurationInMinutes);

            if (!result.IsSuccess)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, result.Errors.FirstOrDefault()?.Message ?? "Failed to add tour duration"));
            }

            _logger.LogInformation("Tour duration successfully added for TourId {TourId}", request.TourId);

            return Task.FromResult(new Google.Protobuf.WellKnownTypes.Empty());
        }

        //publichovanej ture
        public override Task<Google.Protobuf.WellKnownTypes.Empty> PublishTour(PublishTourRequest request,ServerCallContext context)
        {
            _logger.LogInformation("PublishTour called for TourId {TourId}", request.TourId);

            var result = _tourService.Publish(request.TourId);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault()?.Message ?? "Failed to publish tour";
                throw new RpcException(new Status(StatusCode.InvalidArgument, errorMessage));
            }

            _logger.LogInformation("Tour {TourId} successfully published.", request.TourId);

            return Task.FromResult(new Google.Protobuf.WellKnownTypes.Empty());
        }

        //arhiviranje ture
        public override Task<Empty> ArchiveTour(ArchiveTourRequest request, ServerCallContext context)
        {
            _logger.LogInformation("ArchiveTour called for TourId {TourId} by User {UserId}", request.TourId, request.UserId);

            var result = _tourService.Archive(request.TourId, request.UserId);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault()?.Message ?? "Failed to archive tour.";
                throw new RpcException(new Status(StatusCode.PermissionDenied, errorMessage));
            }

            _logger.LogInformation("Tour {TourId} successfully archived by User {UserId}.", request.TourId, request.UserId);

            return Task.FromResult(new Empty());
        }

        //ponovna aktivacija ture
        public override Task<Empty> ReactivateTour(ReactivateTourRequest request, ServerCallContext context)
        {
            _logger.LogInformation("ReactivateTour called for TourId {TourId} by User {UserId}", request.TourId, request.UserId);

            var result = _tourService.Reactivate(request.TourId, request.UserId);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Errors.FirstOrDefault()?.Message ?? "Failed to reactivate tour.";
                throw new RpcException(new Status(StatusCode.PermissionDenied, errorMessage));
            }

            _logger.LogInformation("Tour {TourId} successfully reactivated by User {UserId}.", request.TourId, request.UserId);

            return Task.FromResult(new Empty());
        }

        //dobavi publishovane ture sa samo prvom kljucnom tackom
        public override Task<GetPublishedToursForTouristsResponse> GetPublishedToursForTourists(
    Empty request, ServerCallContext context)
        {
            _logger.LogInformation("GetPublishedToursForTourists called");

            var result = _tourService.GetPublishedForTourists();

            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "No published tours found."));
            }

            var response = new GetPublishedToursForTouristsResponse();
            response.Tours.AddRange(result.Value.Select(t => new TourWithKeyPoints
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Difficulty = t.Difficulty,
                Tags = { t.Tags.Select(tag => tag.ToString()) },
                Status = t.Status.ToString(),
                Price = t.Price,
                UserId = t.UserId,
                LengthInKm = t.LengthInKm,
                PublishedTime = t.PublishedTime.ToString("o"),
                ArchiveTime = t.ArchiveTime?.ToString("o") ?? "",
                KeyPoints = {
            t.KeyPoints.Select(kp => new KeyPoint
            {
                Id = kp.Id,
                Name = kp.Name,
                Longitude = kp.Longitude,
                Latitude = kp.Latitude,
                Description = kp.Description,
                Image = kp.Image,
                TourId = kp.TourId,
                PublicStatus = (int)kp.PublicStatus
            })
        }
            }));

            return Task.FromResult(response);
        }

        public override Task<GetToursByUserResponse> GetAllTours( Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            _logger.LogInformation("GetAllTours called.");

            var result = _tourService.GetAllTours();

            if (!result.IsSuccess || result.Value == null)
                throw new RpcException(new Status(StatusCode.NotFound, "No tours found."));

            var response = new GetToursByUserResponse();
            response.Tours.AddRange(result.Value.Select(t => new GrpcServiceTranscoding.Tour
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Difficulty = t.Difficulty,
                Tags = { t.Tags.Select(tag => tag.ToString()) },
                Status = t.Status.ToString(),
                Price = t.Price,
                UserId = t.UserId,
                LengthInKm = t.LengthInKm,
                PublishedTime = t.PublishedTime.ToString("o"),
                ArchiveTime = t.ArchiveTime?.ToString("o") ?? ""
            }));

            return Task.FromResult(response);
        }







    }

}
