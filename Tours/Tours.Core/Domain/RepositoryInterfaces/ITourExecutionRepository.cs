using System.Collections.Generic;
using Tours.Core.Domain;

namespace Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourExecutionRepository
    {
        TourExecution? Get(long id);
        TourExecution Create(TourExecution execution);
        TourExecution Update(TourExecution execution);
        void Delete(long id);

        // korisne metode:
        bool KeyPointExists(long keyPointId);
        ICollection<KeyPoint> GetKeyPointsByTourId(long tourId);

        TourExecution? GetActiveByTourist(long touristId);
        TourExecution? GetByTourAndTourist(long touristId, long tourId);

        bool IsTourCompleted(long touristId, long tourId);
        List<long> GetAllCompletedToursForUser(long touristId);
    }
}
