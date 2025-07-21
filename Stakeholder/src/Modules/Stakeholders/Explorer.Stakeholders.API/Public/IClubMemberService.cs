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
    public interface IClubMemberService
    {
        Result<ClubMemberDto> Create(ClubMemberDto dto);
        Result<ClubMemberDto> Update(ClubMemberDto dto);
        Result<ClubMemberDto> GetByUserId(long id);
    }
}
