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
    public class ClubTourService : CrudService<ClubTourDto, ClubTour>, IClubTourService
    {
        private readonly IClubTourRepository _clubTourRepository;
        private readonly IMapper _mapper;

        public ClubTourService(ICrudRepository<ClubTour> repository, IMapper mapper, IClubTourRepository clubTourRepository) : base(repository, mapper)
        {
            _clubTourRepository = clubTourRepository;
            _mapper = mapper;


        }

        public Result<ClubTourDto> GetById(long id)
        {
            try
            {
                var clubTour = _clubTourRepository.GetById(id);
                if (clubTour == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Club with ID {id} not found.");
                }
               
                var clubDto = _mapper.Map<ClubTourDto>(clubTour);
                return Result.Ok(clubDto);
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while retrieving the club.")
                             .WithError(e.Message);
            }
        }
    }
}
