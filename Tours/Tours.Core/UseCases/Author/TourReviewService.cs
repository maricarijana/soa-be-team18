using AutoMapper;
using System.Dynamic;
using FluentResults;
using Tours.Application.Dtos;
using Tours.Core.Domain;
using Tours.Application.Public.Author;
using Tours.Core.Domain.RepositoryInterfaces;

namespace Tours.Core.UseCases.Author
{
    public class TourReviewService : CrudService<TourReviewDto, TourReview>, ITourReviewService
    {

        private readonly ITourReviewRepository _tourReviewRepository;
        public TourReviewService(ICrudRepository<TourReview> repository, IMapper mapper, ITourReviewRepository tourReviewRepository) : base(repository, mapper)
        {
            _tourReviewRepository = tourReviewRepository;
        }

        public Result<TourReviewDto> Get(long userId, long tourId)
        {

            var tourReview = _tourReviewRepository.Get(userId, tourId);


            if (tourReview == null)
            {

                return Result.Ok<TourReviewDto>(null);
            }

            var tourReviewDto = new TourReviewDto()
            {
                Id = tourReview.Id,
                IdTour = tourReview.IdTour,
                IdTourist = tourReview.IdTourist,
                Comment = tourReview.Comment,
                Rating = tourReview.Rating,
                DateTour = tourReview.DateTour,
                DateComment = tourReview.DateComment,
                Images = tourReview.Images,
                //Images = string.IsNullOrEmpty(tourReview.Images)
                //? new List<string>()
                //: tourReview.Images.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()


            };
            return Result.Ok(tourReviewDto);
        }
        public Result<List<TourReviewDto>> GetByTourId(long tourId)
        {
            var reviews = _tourReviewRepository.GetByTourId(tourId);

            if (reviews == null || !reviews.Any())
                return Result.Ok(new List<TourReviewDto>());

            var reviewDtos = reviews.Select(r => new TourReviewDto
            {
                Id = r.Id,
                IdTour = r.IdTour,
                IdTourist = r.IdTourist,
                Rating = r.Rating,
                Comment = r.Comment,
                DateTour = r.DateTour,
                DateComment = r.DateComment,
                Images = r.Images
            }).ToList();

            return Result.Ok(reviewDtos);
        }




    }
}
