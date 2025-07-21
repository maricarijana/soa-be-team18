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
    public interface IUserService
    {
        Result<UserDto> Create(UserDto dto);
        Result<UserDto> Update(UserDto dto);
        Result<PagedResult<UserDto>> GetPaged(int page, int pageSize);
        Result Delete(int id);
        Result<UserDto> GetUsername(long id);
        long GetPersonId(long userId);
    }
}
