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
    public class ClubService : CrudService<ClubDto, Club>, IClubService
    {
        private readonly IClubRepository _clubRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClubInvitationRepository _clubInvitationRepository;
        private readonly IMapper _mapper;




        public ClubService(ICrudRepository<Club> repository, IMapper mapper,IUserRepository userRepository, IClubRepository clubRepository,IClubInvitationRepository clubInvitationRepository) : base(repository, mapper) 
        {
            _clubRepository= clubRepository;
            _userRepository= userRepository;
            _clubInvitationRepository= clubInvitationRepository;
            _mapper= mapper;
            

        }
      
        public Result<List<long>> GetUserIds(int clubId)
        {
            try
            {
                var club = CrudRepository.Get(clubId);
                if (club == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Club with ID {clubId} not found.");
                }

                return Result.Ok(club.UserIds);
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while retrieving user IDs.")
                             .WithError(e.Message);
            }
        }
        public Result DeleteMember(long memberId, int clubId, int userId)
        {
            try
            {
                
                var club = CrudRepository.Get(clubId);
                if (club == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Club with ID {clubId} not found.");
                }

                if (club.UserId != userId)
                {
                    return Result.Fail(FailureCode.Forbidden)
                                 .WithError("Only the owner of the club can remove members.");
                }

                bool removed = club.UserIds.Remove(memberId);
                if (!removed)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Member with ID {memberId} not found in the club.");
                }

                
                var updatedClub = CrudRepository.Update(club);

                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail("Result failed");
            }
        }

        public Result<List<UserDto>> GetActiveUsersInClub(int clubId)
        {
            try
            {
                var club = _clubRepository.GetById(clubId);
                if (club == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Club with ID {clubId} not found.");
                }

                var activeUsers = _userRepository.GetActiveUsers();

                var usersInClub = activeUsers
           .Where(user => club.UserIds.Contains(user.Id))
           .Select(user => _mapper.Map<UserDto>(user))  // Koristimo AutoMapper
           .ToList();

                return Result.Ok(usersInClub);
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while retrieving users.")
                             .WithError(e.Message);
            }
        }

        public Result<List<UserDto>> GetUsersOutClub(int clubId)
        {
            try
            {
                var club = _clubRepository.GetById(clubId);
                if (club == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Club with ID {clubId} not found.");
                }

                var activeUsers = _userRepository.GetActiveUsers();

                var eligibleUsers = activeUsers
                    .Where(user => !club.UserIds.Contains(user.Id) && user.Id != club.UserId)
                    .Where(user => IsUserInvitedToClub((int)(user.Id), clubId))
                    .Select(user => _mapper.Map<UserDto>(user))
                    .ToList();

                return Result.Ok(eligibleUsers);
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while retrieving eligible users.")
                             .WithError(e.Message);
            }
        }

        public Result<List<UserDto>> GetEligibleUsersForClub(int clubId)
        {
            try
            {
                var club = _clubRepository.GetById(clubId);
                if (club == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Club with ID {clubId} not found.");
                }

                var activeUsers = _userRepository.GetActiveUsers();

                var eligibleUsers = activeUsers
                    .Where(user => !club.UserIds.Contains(user.Id) && user.Id != club.UserId)
                    .Where(user => !IsUserInvitedToClub((int)(user.Id), clubId))
                    .Select(user => _mapper.Map<UserDto>(user))
                    .ToList();

                return Result.Ok(eligibleUsers);
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while retrieving eligible users.")
                             .WithError(e.Message);
            }
        }

        public bool IsUserInvitedToClub(int userId, int clubId)
        {
            try
            {
                var invitation = _clubInvitationRepository
                                .GetInvitationsByClubId(clubId)
                                .FirstOrDefault(invite => invite.MemberId == userId && invite.ClubId == clubId);
                return invitation != null;
            }
            catch (Exception e)
            {
               
                return false;
            }
        }

        public Result<List<UserDto>> GetUsersForClubInvite(int clubId)
        {
            try
            {
                var club = _clubRepository.GetById(clubId);
                if (club == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Club with ID {clubId} not found.");
                }

                var activeUsers = _userRepository.GetActiveUsers();

                var eligibleUsers = activeUsers
                    .Where(user => !club.UserIds.Contains(user.Id) && user.Id != club.UserId)
                    .Select(user => _mapper.Map<UserDto>(user))
                    .ToList();

                return Result.Ok(eligibleUsers);
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while retrieving eligible users.")
                             .WithError(e.Message);
            }
        }

        public Result AddMember(long memberId, int clubId, int userId)
        {
            try
            {
                var club = CrudRepository.Get(clubId);
                if(club == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError($"Club with ID {clubId} not found.");
                }

                if (club.UserId != userId)
                {
                    return Result.Fail(FailureCode.Forbidden)
                                 .WithError("Only the owner of the club can add members.");
                }

                if (club.UserIds.Contains(memberId))
                {
                    return Result.Fail(FailureCode.NotFound)
                          .WithError($"Member with ID {memberId} is already a member in the club.");
                }

                club.UserIds.Add(memberId);
                var updatedClub = CrudRepository.Update(club);

                return Result.Ok();

            }
            catch (Exception e)
            {
                return Result.Fail("Result failed");
            }
        }
        public Result<ClubDto> GetById(long id)
        {
            try
            {
                var club = _clubRepository.GetById(id);
                if (club == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError($"Club with ID {id} not found.");
                }

                var clubDto = _mapper.Map<ClubDto>(club);
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
