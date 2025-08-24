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
    public interface IClubJoinRequestService
    {
        Result<ClubJoinRequestDto> Create(ClubJoinRequestDto dto);
        Result<ClubJoinRequestDto> Update(ClubJoinRequestDto dto);
        Result<PagedResult<ClubJoinRequestDto>> GetPaged(int page, int pageSize);
        Result Delete(int id); //crud
        Result DeleteMember(int memberId);
        Result<List<ClubJoinRequestDto>> GetRequestsForClubMembers(int clubId);
        bool UserRequestExists(int clubId, int userId);
    }
}
