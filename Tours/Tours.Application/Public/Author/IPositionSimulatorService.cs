using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Application.Dtos;

namespace Tours.Application.Public.Author;

public interface IPositionSimulatorService
{
    Result<PositionSimulatorDto> Create(PositionSimulatorDto positionSimulatorDto);
    Result<PositionSimulatorDto> Update(PositionSimulatorDto positionSimulatorDto);
    Result<PositionSimulatorDto> Get(int id);

    Result<PositionSimulatorDto> GetByTouristId(long touristId);
    public Result<PositionSimulatorDto> UpdatePosition(PositionSimulatorDto dto);

}
