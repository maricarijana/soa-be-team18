using Grpc.Core;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using Tours.Core.UseCases;
//using Tours.Core.UseCases.Author;
using GrpcServiceTranscoding;
using Tours.Core.UseCases.Author;


namespace Tours.API.Controllers
{
    public class PositionSimulatorProtoController : PositionSimulatorService.PositionSimulatorServiceBase
    {
        private readonly IPositionSimulatorService _positionService;

        public PositionSimulatorProtoController(IPositionSimulatorService positionService)
        {
            _positionService = positionService;
        }

        public override Task<PositionSimulatorResponse> GetPosition(
            PositionSimulatorRequest request,
            ServerCallContext context)
        {
            var result = _positionService.GetByTouristId(request.TouristId);
            if (result.IsFailed || result.Value == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Position not found."));

            var pos = result.Value;
            return Task.FromResult(new PositionSimulatorResponse
            {
                TouristId = pos.TouristId,
                Latitude = pos.Latitude,
                Longitude = pos.Longitude,
                UpdatedAt = pos.UpdatedAt.ToString("O")
            });
        }

        public override Task<PositionSimulatorResponse> UpdatePosition(
            UpdatePositionRequest request,
            ServerCallContext context)
        {
            var dto = new PositionSimulatorDto
            {
                TouristId = request.TouristId,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            var result = _positionService.UpdatePosition(dto);
            if (result.IsFailed)
                throw new RpcException(new Status(StatusCode.Internal, "Failed to update position."));

            var pos = result.Value;
            return Task.FromResult(new PositionSimulatorResponse
            {
                TouristId = pos.TouristId,
                Latitude = pos.Latitude,
                Longitude = pos.Longitude,
                UpdatedAt = pos.UpdatedAt.ToString("O") // "O" = ISO 8601 format (npr. 2025-10-07T22:33:00Z)
            });
        }
    }
}
