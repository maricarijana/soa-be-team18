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
    public interface IClubService
    {
        Result<ClubDto> Create(ClubDto dto);
        Result<ClubDto> Update(ClubDto dto);
        Result<PagedResult<ClubDto>> GetPaged(int page, int pageSize);
        Result Delete(int id);
        Result AddMember(long memberId, int clubId, int userId);
        Result DeleteMember(long memberId, int clubId, int userId);
        Result<List<long>> GetUserIds(int clubId);
        Result<List<UserDto>> GetActiveUsersInClub(int clubId);
        Result<List<UserDto>> GetUsersOutClub(int clubId);
        Result<List<UserDto>> GetEligibleUsersForClub(int clubId);
        Result<ClubDto> GetById(long id);

    }
}
