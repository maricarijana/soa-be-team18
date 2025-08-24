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
    public interface IClubInvitationService
    {
        Result<PagedResult<ClubInvitationDto>> GetPaged(int page, int pageSize);
        Result<ClubInvitationDto> Create(ClubInvitationDto invitation);
        Result<ClubInvitationDto> Update(ClubInvitationDto invitation);
        Result Delete(int id);
        int GetMaxId();
        Result<List<ClubInvitationDto>> GetInvitationsByClubId(long clubId);

    }
}
