using Grpc.Core;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using System.Net;
using GrpcServiceTranscoding;

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
    .Select(t => Enum.TryParse<TourTags>(t, true, out var tag) ? tag : default)
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
    }

    }
