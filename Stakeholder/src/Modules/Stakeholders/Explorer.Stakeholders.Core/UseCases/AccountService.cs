using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases
{

    public class AccountService : CrudService<AccountDto, User>, IAccountService
    {
        public readonly ICrudRepository<User> _userRepository;
        public readonly ICrudRepository<Person> _personRepository;

        public AccountService(ICrudRepository<User> userRepository,ICrudRepository<Person> personRepository
            ,  IMapper mapper) : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public new Result<PagedResult<AccountDto>> GetPagedAccount(int page, int pageSize)
        {
            var mappedResults = GetPaged(page, pageSize);

            if (mappedResults.IsFailed)
            {
                return Result.Fail(mappedResults.Errors);
            }

            var pagedAccounts = mappedResults.Value;
            Person? personResult;
            AccountDto? userResult;

            var personList = _personRepository.GetPaged(0, 0).Results;

            foreach (var account in pagedAccounts.Results)
            {
                try
                {
                    userResult = mappedResults.Value.Results.Find(x => x.Username == account.Username);
                    personResult = personList.Find(p => p.UserId == userResult.Id);

                    account.Email = personResult?.Email ?? "N/A";
                }
                catch (Exception ex)
                {
                    if (account.Role != UserRole.Administrator.ToString())
                    {
                        return Result.Fail($"An error occurred while retrieving the person for account " +
                            $"{account.Id}: {ex.Message}");
                    }

                    account.Email = "N/A";
                    continue;
                }
            }

            return Result.Ok(pagedAccounts);
        }


      
        public Result<AccountDto> BlockUser(long accountId)
        {

            var user = _userRepository.Get(accountId);
            //if (user == null)
            //{
            //    return Result.Fail(new Error("User not found.").WithMetadata("status", 404));
            //}

            if (!user.IsActive)
            {
                return MapToDto(user); 
            }

            user.IsActive = false;
            try
            {
                var updatedUser = _userRepository.Update(user);

                return MapToDto(updatedUser);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while updating the user: {ex.Message}");
            }
        }
    }
}
