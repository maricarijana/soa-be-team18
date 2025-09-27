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
                Name = request.Name,
                Description = request.Description,
                Difficulty = request.Difficulty,
                Tags = null,
                Status = 0,
                Price = 0,
                UserId = request.UserId,
                LengthInKm = 0,
                PublishedTime = DateTime.Now,
                ArchiveTime = DateTime.Now,
            };

            var result = _tourService.Create(dto);

            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, result.Errors.FirstOrDefault()?.Message ?? "Failed to add tour"));
            }

            return Task.FromResult(new Tour
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                Difficulty = result.Value.Difficulty,
                Tags = {  },
                Status = " ",
                Price = (double)result.Value.Price,
                UserId = result.Value.UserId,
                LengthInKm = (double)result.Value.LengthInKm,
                PublishedTime = " ",
                ArchiveTime = " "
            });
        }


    }
}
