using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class PersonService : CrudService<PersonDto, Person>, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICrudRepository<Person> _crudRepository;
        public PersonService(ICrudRepository<Person> crudRepository, IMapper mapper, IPersonRepository personRepository) : base(crudRepository, mapper)
        {
            _personRepository = personRepository;
            _crudRepository = crudRepository;   
        }

        public Result<PersonDto> AddXP(long id, int xp)
        {
            var person = _personRepository.GetByUserId(id);
            if (person == null)
            {
                return Result.Fail("Person not found!");
            }
            person.AddXP(xp);
            _crudRepository.Update(person);
            return Result.Ok(MapToDto(person));
            
        }
    }
}
