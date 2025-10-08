using AutoMapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using Tours.Core.Domain;
using Tours.Core.Domain.Execution;

namespace Tours.Core.UseCases.Author;

public class PositionSimulationService : CrudService<PositionSimulatorDto, PositionSimulator>, IPositionSimulatorService
{
    IPositionSimulatorRepository _positionSimulatorRepository { get; set; }
    public PositionSimulationService(IPositionSimulatorRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _positionSimulatorRepository = repository;
    }


    public Result<PositionSimulatorDto> GetByTouristId(long touristId)
    {
        var positionDto = MapToDto(_positionSimulatorRepository.GetByTouristId(touristId));

        if (positionDto == null)
        {
            return Result.Fail("No position found");
        }

        return Result.Ok(positionDto);
    }
    public Result<PositionSimulatorDto> UpdatePosition(PositionSimulatorDto dto)
    {
        var existing = _positionSimulatorRepository.GetByTouristId(dto.TouristId);

        if (existing == null)
        {
            var newPosition = new PositionSimulator(dto.Latitude, dto.Longitude, dto.TouristId);
            _positionSimulatorRepository.Create(newPosition);
            return Result.Ok(MapToDto(newPosition));
        }
        else
        {
            existing.Update(dto.Latitude, dto.Longitude);
            _positionSimulatorRepository.Update(existing);
            return Result.Ok(MapToDto(existing));
        }
    }
}
