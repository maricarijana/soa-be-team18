using AutoMapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using Tours.Core.Domain.RepositoryInterfaces;
using Tours.Core.Domain;

namespace Tours.Core.UseCases.Author
{
    public class TourExecutionService : ITourExecutionService
    {
        private readonly ITourExecutionRepository _repository;
        private readonly IMapper _mapper;

        public TourExecutionService(ITourExecutionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Result<TourExecutionDto> Create(TourExecutionDto executionDto)
        {
            var execution = _mapper.Map<TourExecution>(executionDto);
            execution = new TourExecution(execution.TourId, execution.TouristId); // uvek krećemo novu sesiju

            var created = _repository.Create(execution);
            if (created == null) return Result.Fail<TourExecutionDto>("Failed to create tour execution.");

            var dto = _mapper.Map<TourExecutionDto>(created);
            return Result.Ok(dto);
        }

        public Result<TourExecutionDto> CompleteTourExecution(long id)
        {
            var execution = _repository.Get(id);
            if (execution == null)
                return Result.Fail<TourExecutionDto>($"Tour execution {id} not found.");

            execution.CompleteTour();
            var updated = _repository.Update(execution);

            return Result.Ok(_mapper.Map<TourExecutionDto>(updated));
        }

        public Result<TourExecutionDto> AbandonTourExecution(long id)
        {
            var execution = _repository.Get(id);
            if (execution == null)
                return Result.Fail<TourExecutionDto>($"Tour execution {id} not found.");

            execution.AbandonTour();
            var updated = _repository.Update(execution);

            return Result.Ok(_mapper.Map<TourExecutionDto>(updated));
        }

        public Result<TourExecutionDto> CompleteKeyPoint(long executionId, long keyPointId)
        {
            var execution = _repository.Get(executionId);
            if (execution == null)
                return Result.Fail<TourExecutionDto>($"Tour execution {executionId} not found.");

            if (!_repository.KeyPointExists(keyPointId))
                return Result.Fail<TourExecutionDto>($"Key point {keyPointId} does not exist.");

            try
            {
                execution.CompleteKeyPoint(keyPointId);
                _repository.Update(execution);
                return Result.Ok(_mapper.Map<TourExecutionDto>(execution));
            }
            catch (ArgumentException ex)
            {
                return Result.Fail<TourExecutionDto>(ex.Message);
            }
        }

        public void UpdateLastActivity(long executionId)
        {
            var execution = _repository.Get(executionId);
            if (execution == null)
                throw new Exception($"Execution {executionId} not found.");

            execution.UpdateLastActivity();
            _repository.Update(execution);
        }

        public Result<TourExecutionDto> GetByTourAndTouristId(long touristId, long tourId)
        {
            var execution = _repository.GetByTourAndTourist(touristId, tourId);
            if (execution == null)
                return Result.Fail<TourExecutionDto>("No execution found for given tour and tourist.");

            return Result.Ok(_mapper.Map<TourExecutionDto>(execution));
        }

        public ICollection<KeyPointDto> GetKeyPointsForTour(long tourId)
        {
            var keyPoints = _repository.GetKeyPointsByTourId(tourId);
            var dtos = keyPoints.Select(kp => new KeyPointDto
            {
                Id = kp.Id,
                Name = kp.Name,
                Latitude = kp.Latitude,
                Longitude = kp.Longitude,
                Description = kp.Description
            }).ToList();

            return dtos;
        }

        public Result<TourExecutionDto> GetActiveTourByTouristId(long touristId)
        {
            var execution = _repository.GetActiveByTourist(touristId);
            if (execution == null)
                return Result.Fail<TourExecutionDto>($"No active tour found for tourist {touristId}.");

            return Result.Ok(_mapper.Map<TourExecutionDto>(execution));
        }
    }
}
