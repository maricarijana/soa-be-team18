using Tours.Application.Dtos;
using FluentResults;

namespace Tours.Application.Public.Author;

public interface IKeyPointService
{
    Result<KeyPointDto> Create(KeyPointDto keyPointDto);
    Result<KeyPointDto> Update(KeyPointDto keyPointDto);
    Result Delete(int id);
    Result<List<KeyPointDto>> GetByUserId(long userId);
    Result<KeyPointDto> Get(int id);
    int GetMaxId(long userId);
    Result<List<KeyPointDto>> GetRequestedPublic();
    //Result<List<KeyPointDto>> GetByCoordinated(long v1, long v2, int v3);
}