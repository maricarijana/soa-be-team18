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
using static Explorer.Stakeholders.Core.UseCases.ClubInvitationService;

namespace Explorer.Stakeholders.Core.UseCases
{
   
    public class ClubInvitationService : CrudService<ClubInvitationDto, ClubInvitation>, IClubInvitationService
    {

        private readonly ICrudRepository<Person> _personRepository;
        private readonly IClubInvitationRepository _clubInvitationRepository;

        public ClubInvitationService(ICrudRepository<ClubInvitation> repository, IMapper mapper, ICrudRepository<Person> personRepository, IClubInvitationRepository clubInvitationRepository) : base(repository, mapper)
        {

            _personRepository = personRepository;
            _clubInvitationRepository = clubInvitationRepository;
        }

        public Result<List<ClubInvitationDto>> GetInvitationsByClubId(long clubId)
        {
            var allInviattions = _clubInvitationRepository.GetInvitationsByClubId(clubId).ToList();
            return MapToDto(allInviattions);
        }


        public int GetMaxId()
        {
            return _clubInvitationRepository.GetMaxId();
        }
      /* public Result<List<ClubInvitationDto>> GetInvitationsByClubId(long clubId)
{
    var invitations = _clubInvitationRepository.GetInvitationsByClubId(clubId);
    
    // Mapiraj liste koristeći MapToDto i vrati kao Result.
    var invitationDtos = MapToDto(invitations);
    
    return Result.Ok(invitationDtos);
}*/

        
    }
   
}
