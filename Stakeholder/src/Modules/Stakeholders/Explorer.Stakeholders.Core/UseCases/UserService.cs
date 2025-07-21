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
        public UserService(ICrudRepository<User> repository, IUserRepository userRepository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public Result<UserDto> GetUsername(long id)
        {
            var user = _repository.Get(id);
            if(user != null)
            {
                return Result.Ok(new UserDto(user.Username));
            }
            return Result.Fail("error geting user");
        }

        public long GetPersonId(long userId)
        {
            var personId= _userRepository.GetPersonId(userId);
            return personId;
        }
    }
}
