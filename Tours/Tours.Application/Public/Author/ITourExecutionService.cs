using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Application.Dtos;

namespace Tours.Application.Public.Author
{
    public interface ITourExecutionService
    {
        Result<TourExecutionDto> Create(TourExecutionDto execution);
        Result<TourExecutionDto> CompleteTourExecution(long id);
        Result<TourExecutionDto> AbandonTourExecution(long id);
        Result<TourExecutionDto> CompleteKeyPoint(long executionId, long keyPointId);
        void UpdateLastActivity(long executionId);
        Result<TourExecutionDto> GetByTourAndTouristId(long touristId, long tourId);
        ICollection<KeyPointDto> GetKeyPointsForTour(long tourId);
        Result<TourExecutionDto> GetActiveTourByTouristId(long touristId);
    }
}
