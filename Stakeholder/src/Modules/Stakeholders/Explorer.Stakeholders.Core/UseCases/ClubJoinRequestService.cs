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
    public class ClubJoinRequestService : CrudService<ClubJoinRequestDto, ClubJoinRequest>, IClubJoinRequestService
    {
        private readonly IClubJoinRequestRepository _clubJoinRequestRepository;
        public ClubJoinRequestService(ICrudRepository<ClubJoinRequest> repository, IMapper mapper, IClubJoinRequestRepository clubJoinRequestRepository) : base(repository, mapper) {
            _clubJoinRequestRepository = clubJoinRequestRepository;
        }

        public Result<List<ClubJoinRequestDto>> GetRequestsForClubMembers(int clubId)
        {
            var allRequests = _clubJoinRequestRepository.GetAll();

            var acceptedRequests = allRequests
            .Where(r => r.Status == Domain.JoinRequestStatus.ACCEPTED && r.ClubId == clubId)
            .ToList();

            return MapToDto(acceptedRequests);
        }

        //lista usera da dobavi
        //napracvim user controller

        public Result DeleteMember(int requestId)
        {
            try
            {
                var request = _clubJoinRequestRepository.GetById(requestId);

                if (request == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Request with ID {requestId} not found.");
                }

                if (request.Status != Domain.JoinRequestStatus.ACCEPTED)
                {
                    return Result.Fail("Only requests with ACCEPTED status can be deleted.");
                }

                _clubJoinRequestRepository.Delete(request);
                
                

                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail("Exception");//(FailureCode.Error).WithError(e.Message);
            }
        }
        public bool UserRequestExists(int clubId, int userId)
        {
            var request = _clubJoinRequestRepository.GetAll().FirstOrDefault(r => r.ClubId == clubId && r.UserId == userId);
            return request != null;

        }
    }

}
