using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class ClubMemberService : CrudService<ClubMemberDto, ClubMember>, IClubMemberService
    {
        private readonly IClubMemberRepository _clubMemberRepository;
        private readonly IMapper _mapper;
        public ClubMemberService(ICrudRepository<ClubMember> repository, IClubMemberRepository clubMemberRepository, IMapper mapper) : base(repository, mapper)
        {
            _clubMemberRepository = clubMemberRepository;
            _mapper = mapper;
        }

        public Result<ClubMemberDto> GetByUserId(long id)
        {
            var clubMember = _clubMemberRepository.GetByUserId(id);
            if (clubMember == null)
            {
                return Result.Fail<ClubMemberDto>($"ClubMember with ID {id} not found.");
            }

            var clubMemberDto = _mapper.Map<ClubMemberDto>(clubMember);
            return Result.Ok(clubMemberDto);
        }

       /* public Result<ClubMemberDto> Create(ClubMemberDto dto)
        {
            try
            {
                var clubMember = _mapper.Map<ClubMember>(dto);
                CrudRepository.Create(clubMember);

                var createdDto = _mapper.Map<ClubMemberDto>(clubMember);
                return Result.Ok(createdDto);
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while creating the club member.")
                             .WithError(ex.Message);
            }
        }

        public Result<ClubMemberDto> Update(ClubMemberDto dto)
        {
            try
            {
                var existingMember = _clubMemberRepository.GetByUserId(dto.Id);
                if (existingMember == null)
                {
                    return Result.Fail<ClubMemberDto>($"Club member with ID {dto.Id} not found.");
                }

                _mapper.Map(dto, existingMember);
                CrudRepository.Update(existingMember);

                var updatedDto = _mapper.Map<ClubMemberDto>(existingMember);
                return Result.Ok(updatedDto);
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while updating the club member.")
                             .WithError(ex.Message);
            }
        }*/

    }
}
