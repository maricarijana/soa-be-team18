using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public  interface IClubTourService
    {
        Result<ClubTourDto> Create(ClubTourDto dto);
        Result<ClubTourDto> Update(ClubTourDto dto);
        Result<PagedResult<ClubTourDto>> GetPaged(int page, int pageSize);
        Result<ClubTourDto> GetById(long id);
    }
}
