using AutoMapper;
using Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using Tours.Application.Dtos;
using Tours.Core.Domain;
using Tours.Application.Public.Author;

namespace Tours.Core.UseCases.Author;

public class KeyPointService : CrudService<KeyPointDto, KeyPoint>, IKeyPointService
{
    IKeyPointRepository _keyPointRepository { get; set; }
    public KeyPointService(ICrudRepository<KeyPoint> repository, IMapper mapper, IKeyPointRepository keyPointRepository) : base(repository, mapper)
    {
        _keyPointRepository = keyPointRepository;
    }
    public Result<List<KeyPointDto>> GetByUserId(long userId)
    {

        {
            var keyPoints = _keyPointRepository.GetKeyPointsByUserId(userId);

            if (keyPoints == null || keyPoints.Count == 0)
            {
                return Result.Fail<List<KeyPointDto>>("No keypoints found for the specified user.");
            }

            var keyPointDtos = keyPoints.Select(kp => new KeyPointDto
            {
                Id = kp.Id,
                Name = kp.Name,
                Longitude = kp.Longitude,
                Latitude = kp.Latitude,
                Description = kp.Description,
                Image = kp.Image,
                UserId = kp.UserId,
                PublicStatus = (Application.Dtos.PublicStatus)kp.PublicStatus,
                TourId = kp.TourId,



            }).ToList();

            return Result.Ok(keyPointDtos);

        }
    }

    public int GetMaxId(long userId)
    {
        return _keyPointRepository.GetMaxId(userId);
    }

    public Result<List<KeyPointDto>> GetRequestedPublic()
    {
        var keyPoints = _keyPointRepository.GetAll().FindAll(kp => kp.PublicStatus == Domain.PublicStatus.REQUESTED_PUBLIC);

        var keyPointDtos = keyPoints.Select(kp => new KeyPointDto
        {
            Id = kp.Id,
            Name = kp.Name,
            Longitude = kp.Longitude,
            Latitude = kp.Latitude,
            Description = kp.Description,
            Image = kp.Image,
            UserId = kp.UserId,
            TourId = kp.TourId,
            PublicStatus = (Application.Dtos.PublicStatus)kp.PublicStatus



        }).ToList();

        return Result.Ok(keyPointDtos);
    }
    public Result<List<KeyPointDto>> GetByTourId(long tourId)
    {
        var keyPoints = _keyPointRepository.GetKeyPointsByTourId(tourId);

        if (keyPoints == null || keyPoints.Count == 0)
        {
            return Result.Fail<List<KeyPointDto>>("No keypoints found for the specified tour.");
        }

        var keyPointDtos = keyPoints.Select(kp => new KeyPointDto
        {
            Id = kp.Id,
            Name = kp.Name,
            Longitude = kp.Longitude,
            Latitude = kp.Latitude,
            Description = kp.Description,
            Image = kp.Image,
            UserId = kp.UserId,
            TourId = kp.TourId,
            PublicStatus = (Application.Dtos.PublicStatus)kp.PublicStatus
        }).ToList();

        return Result.Ok(keyPointDtos);
    }

}