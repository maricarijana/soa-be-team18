using AutoMapper;
using AutoMapper.Configuration.Annotations;
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
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly ICrudRepository<User> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public UserService(ICrudRepository<User> repository, IUserRepository userRepository, IPersonRepository personRepository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _mapper = mapper;

        }

        public Result<UserDto> GetUsername(long id)
        {
            var user = _repository.Get(id);
            if (user != null)
            {
                return Result.Ok(new UserDto(user.Username));
            }
            return Result.Fail("error geting user");
        }

        public long GetPersonId(long userId)
        {
            var personId = _userRepository.GetPersonId(userId);
            return personId;
        }

        //kt1-irina
        public Result<UserDto> RegisterWithoutAuth(AccountRegistrationDto account)
        {
            if (_userRepository.Exists(account.Username))
                return Result.Fail(FailureCode.NonUniqueUsername);
            //mzoe da bita turistu ili vodica-admin je u bazi 
            if (account.Role != "Tourist" && account.Role != "Guide")
                return Result.Fail(FailureCode.InvalidArgument).WithError("Only Tourist and Guide roles are allowed.");

            var parsedRole = account.Role == "Tourist" ? UserRole.Tourist : UserRole.Guide;

            try
            {
                var user = _userRepository.Create(new User(account.Username, account.Password, parsedRole, true));
                var person = _personRepository.Create(new Person(user.Id, account.Name, account.Surname, account.Email, account.ProfilePicture, account.Biography, account.Motto));

                var userDto = _mapper.Map<UserDto>(user);
                return Result.Ok(userDto);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result DeactivateUser(long userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null) return Result.Fail("User not found");

            if (user.Role == UserRole.Administrator)
                return Result.Fail("Cannot deactivate administrator accounts.");

            user.IsActive = false;
            _repository.Update(user); // ili posebna metoda ako ne koristiš bazni update

            return Result.Ok();
        }


    }
}
