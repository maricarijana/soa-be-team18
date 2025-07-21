using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public;
public interface IPersonService 
{
    Result<PersonDto> Get(int id);
    Result<PersonDto> Update(PersonDto personDto);
    Result<PersonDto> AddXP(long id,int xp);
}
