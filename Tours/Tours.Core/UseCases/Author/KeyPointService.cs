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
    private readonly ITourRepository _tourRepository;
    private readonly IMapper _mapper;


    public KeyPointService(ICrudRepository<KeyPoint> repository, IMapper mapper, IKeyPointRepository keyPointRepository,ITourRepository tourRepository) : base(repository, mapper)
    {
        _keyPointRepository = keyPointRepository;
        _tourRepository = tourRepository;
        _mapper = mapper;
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
    //racunanje distance izmedju kljucnih tacaka za ture
    public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // poluprečnik Zemlje u km
        double dLat = (lat2 - lat1) * Math.PI / 180;
        double dLon = (lon2 - lon1) * Math.PI / 180;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }



    public int GetMaxId(long userId)
    {
        return _keyPointRepository.GetMaxId(userId);
    }
    public override Result<KeyPointDto> Create(KeyPointDto dto)
    {
        var result = base.Create(dto); // kreiranje kljucne

        if (!result.IsSuccess || result.Value == null)
            return result;

        // sve tacke ture
        var keyPoints = _keyPointRepository.GetKeyPointsByTourId(dto.TourId)
                                           .OrderBy(kp => kp.Id)
                                           .ToList();

        if (keyPoints.Count >= 2)
        {
            // Poslednje dve tacke
            var prev = keyPoints[^2];
            var last = keyPoints[^1];

            // izracunaj rastojanje
            double distance = CalculateDistance(prev.Latitude, prev.Longitude,
                                                last.Latitude, last.Longitude);

            // azuriranje duzine ture
            Console.WriteLine($"DTO TourId: {dto.TourId}");
            var tour = _tourRepository.GetByIdWithKeyPoints(dto.TourId);
            Console.WriteLine($"Tour null? {tour == null}");
            Console.WriteLine($"Mapper null? {_mapper == null}");
            Console.WriteLine($"Tour.KeyPoints null? {tour?.KeyPoints == null}"); if (tour != null)
            {
                //tour.KeyPoints.Add(_mapper.Map<KeyPoint>(dto));
                tour.UpdateLength(distance); 
                _tourRepository.Save();
            }
        }

        return result;
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