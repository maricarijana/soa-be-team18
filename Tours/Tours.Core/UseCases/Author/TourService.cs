using AutoMapper;
using Tours.Core.Domain;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

using TourStatus = Tours.Application.Dtos.TourStatus;
using TourTags = Tours.Application.Dtos.TourTags;

namespace Tours.Core.UseCases.Author
{
    public class TourService : CrudService<TourDto, Tour>, ITourService
    {

        ITourRepository _tourRepository { get; set; }

        IMapper _mapper { get; set; }
        public TourService(ICrudRepository<Tour> repository, IMapper mapper, ITourRepository tourRepository) : base(repository, mapper)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        public Result<List<TourDto>> GetByUserId(long userId)
        {

            {
                var tours = _tourRepository.GetToursByUserId(userId);

                if (tours == null || tours.Count == 0)
                {
                    return Result.Fail<List<TourDto>>("No tours found for the specified user.");
                }

                var tourDtos = tours.Select(t => new TourDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Difficulty = t.Difficulty,
                    Tags = t.Tags.Select(tag => (TourTags)tag).ToList(),
                    UserId = t.UserId,
                    Status = (TourStatus)t.Status,
                    Price = t.Price,
                    EquipmentIds = t.EquipmentIds,
                    LengthInKm = t.LengthInKm,
                    KeyPoints = t.KeyPoints.Select(kp => new KeyPointDto
                    {
                        Id = kp.Id,
                        Name = kp.Name,
                        Longitude = kp.Longitude,
                        Latitude = kp.Latitude,
                        Description = kp.Description,
                        Image = kp.Image,
                        TourId = kp.TourId


                    }).ToList()
                }).ToList();

                return Result.Ok(tourDtos);

            }
        }


        //public Result<PagedResult<EquipmentDto>> GetEquipment(long tourId)
        //{
        //    var equipment = _tourRepository.GetEquipment(tourId);
        //    var result = new PagedResult<Equipment>(equipment, equipment.Count);
        //    var items = result.Results.Select(_mapper.Map<EquipmentDto>).ToList();
        //    return new PagedResult<EquipmentDto>(items, result.TotalCount);
        //}

      
        public Result Archive(long id)
        {
            try
            {
                var tour = _tourRepository.GetById(id);
                tour.Archive(tour.UserId);
                _tourRepository.Save();
                return Result.Ok();
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
            }


        }
        //public Result Archive(long id, long userId)
        //{
        //    try
        //    {
        //        var tour = _tourRepository.GetById(id);
        //        tour.Archive(userId); // koristi proveru IsAuthor(userId)
        //        _tourRepository.Save();
        //        return Result.Ok();
        //    }
        //    catch (ArgumentException e)
        //    {
        //        return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        //    }
        //    catch (UnauthorizedAccessException e)
        //    {
        //        return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
        //    }
        //}

        public Result Reactivate(long id)
        {
            try
            {
                var tour = _tourRepository.GetById(id);
                tour.Reactivate(tour.UserId);
                _tourRepository.Save();
                return Result.Ok();
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
            }
        }

        public Result Publish(long id)
        {
            try
            {
                var tour = _tourRepository.GetByIdWithKeyPoints(id);
                tour.Publish(tour.UserId);
                _tourRepository.Save();
                return Result.Ok();
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
            }


        }

        public Result UpdateDistance(long id, double distance)
        {
            try
            {
                var tour = _tourRepository.GetByIdWithKeyPoints(id);
                tour.UpdateLength(distance);
                _tourRepository.Save();
                return Result.Ok();
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
            }
        }

        public Result<TourDto> GetWithKeyPoints(int tourId)
        {
            try
            {
                var tour = _tourRepository.GetWithKeyPoints(tourId);

                if (tour == null)
                {
                    return Result.Fail<TourDto>("Tour not found.");
                }

                var tourDto = _mapper.Map<TourDto>(tour);

                return Result.Ok(tourDto);
            }
            catch (Exception e)
            {
                return Result.Fail<TourDto>(e.Message);
            }
        }

        public Result DeleteTour(int id)
        {
            return Delete(id);
        }

        public Result GetById(long id)
        {
            var tour = _tourRepository.GetById(id);
         

            return Result.Ok();
        }

        public Result<TourDto> Get(int id)
        {
            try
            {
                var tour = _tourRepository.GetById(id);

                if (tour == null)
                {
                    return Result.Fail<TourDto>("Tour not found.");
                }

                var tourDto = _mapper.Map<TourDto>(tour);

                return Result.Ok(tourDto);
            }
            catch (Exception e)
            {
                return Result.Fail<TourDto>(e.Message);
            }
        }
        public Result<TourDto> GetTourById(long id)
        {
            var tour = _tourRepository.GetById(id);
            return MapToDto(tour);
        }
        public Result AddDuration(long tourId, Application.Dtos.TransportType transport, int durationInMinutes)
        {
            try
            {
                //var tour = _tourRepository.GetById(tourId);
                var tour=_tourRepository.GetByIdWithKeyPoints(tourId);
                if (tour == null)
                {
                    return Result.Fail("Tour not found.");
                }

                var domainTransport = (Tours.Core.Domain.TransportType)transport;
                var duration = new TourDuration(domainTransport, durationInMinutes);
                tour.AddDuration(duration);
                _tourRepository.Save();
                return Result.Ok();
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Result.Fail(FailureCode.Forbidden).WithError(e.Message);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }



        //public Result<List<TourDto>> GetPublised()
        //{
        //    var tours = _tourRepository.GetPublished(0, 0);
        //    return MapToDto(tours);
        //}
    }
}
