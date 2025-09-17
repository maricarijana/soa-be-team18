using AutoMapper;
using FluentResults;
using Geolocation;
using System.Security.Cryptography.X509Certificates;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using Tours.Core.Domain;
using Tours.Core.Domain.RepositoryInterfaces;
using Tours.Core.UseCases;

namespace Tours.Core.UseCases.Author
{
    public class TourOverviewService : ITourOverviewService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IKeyPointRepository _keyPointRepository;


        IMapper _mapper { get; set; }

        //public TourOverviewService(ITourRepository tourRepository, IKeyPointRepository keyPointRepository,
        //    ITourReviewRepository tourReviewRepository, IMapper mapper)
        //{
        //    _tourRepository = tourRepository;
        //    _keyPointRepository = keyPointRepository;
        //    _tourReviewRepository = tourReviewRepository;
        //    _mapper = mapper;

        //}
        public TourOverviewService(ITourRepository tourRepository, IKeyPointRepository keyPointRepository, IMapper mapper)
        {
            _tourRepository = tourRepository;
            _keyPointRepository = keyPointRepository;
            _mapper = mapper;

        }

      

        public Result<List<TourOverviewDto>> GetAllWithoutReviews(int page, int pageSize)
        {
            var publishedTours = _tourRepository.GetPublished(page, pageSize);

            var ret = publishedTours.ToResult();

            if (ret.IsFailed || ret.Value.Results.Any(x => x.KeyPoints.Count() == 0))
            {
                return Result.Fail(ret.Errors);
            }


            var pagedItems = new List<TourOverviewDto>();

            foreach (var tour in ret.Value.Results)
            {
                var tags = tour.Tags.Select(t => t.ToString()).ToList();

                var newTourOverview = new TourOverviewDto()
                {
                    TourId = tour.Id,
                    TourDescription = tour.Description,
                    Tags = tags,
                    TourDifficulty = tour.Difficulty,
                    TourName = tour.Name,
                    Price = Convert.ToDecimal(tour.Price),
                    OriginalPrice = 0,
                    DiscountPercentage = 0,

                    FirstKeyPoint = _mapper.Map<KeyPointDto>(tour.KeyPoints.First()),
                    //Reviews = new List<TourReviewDto>()
                };

                pagedItems.Add(newTourOverview);
            }

            var pagedResult = new PagedResult<TourOverviewDto>(pagedItems, pagedItems.Count());    //morace se zameniti na List

            return Result.Ok();    //i ovde obrisala paged result iz zagrade
        }

        public Result<TourOverviewDto> GetById(int id)
        {
            var tour = _tourRepository.GetWithKeyPoints(id);

            var ret = tour.ToResult();

            if (ret.IsFailed)
            {
                return Result.Fail(ret.Errors);
            }



            var tags = tour.Tags.Select(t => t.ToString()).ToList();

            var newTourOverview = new TourOverviewDto()
            {
                TourId = tour.Id,
                TourDescription = tour.Description,
                Tags = tags,
                TourDifficulty = tour.Difficulty,
                TourName = tour.Name,
                Price = Convert.ToDecimal(tour.Price),
                FirstKeyPoint = _mapper.Map<KeyPointDto>(tour.KeyPoints.First()),
                //Reviews = new List<TourReviewDto>()
            };





            return Result.Ok(newTourOverview);
        }

      
        public Result<List<TourOverviewDto>> GetByCoordinated(double latitude, double longitude, int distance, int page, int pageSize)
        {
            var res = new List<KeyPoint>();
            var centralCoordinate = new Coordinate(latitude, longitude);
            var keyPoints = _keyPointRepository.GetAll();


            foreach (var kp in keyPoints)
            {
                double dist = GeoCalculator.GetDistance(
                centralCoordinate,
                new Coordinate(kp.Latitude, kp.Longitude),
                4,
                DistanceUnit.Kilometers);
                if (dist < distance)
                    res.Add(kp);
            }

            var tours = _tourRepository.GetByKeyPoints(res, 0, 0);
            var ret = tours.ToResult();

            var pagedItems = new List<TourOverviewDto>();
            foreach (var tour in ret.Value.Results)
            {
                var tags = tour.Tags.Select(t => t.ToString()).ToList();

                var newTourOverview = new TourOverviewDto()
                {
                    TourId = tour.Id,
                    TourDescription = tour.Description,
                    Tags = tags,
                    TourDifficulty = tour.Difficulty,
                    TourName = tour.Name,
                    Price = Convert.ToDecimal(tour.Price),
                    FirstKeyPoint = _mapper.Map<KeyPointDto>(tour.KeyPoints.First()),
                    //Reviews = new List<TourReviewDto>()
                };

                pagedItems.Add(newTourOverview);
            }

            var pagedResult = new PagedResult<TourOverviewDto>(pagedItems, pagedItems.Count());

            return Result.Ok();//morace se izmeniti-ne mzoe paged result ovde
        }
    }
}
