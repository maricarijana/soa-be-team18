using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tours.Core.Domain;
using Tours.Core.Domain.RepositoryInterfaces;

namespace Tours.Infrastructure.Database.Repositories
{
    public class TourExecutionRepository : ITourExecutionRepository
    {
        private readonly ToursContext _dbContext;
        private readonly DbSet<TourExecution> _dbSet;

        public TourExecutionRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TourExecution>();
        }

        public TourExecution? Get(long id)
        {
            return _dbSet.FirstOrDefault(te => te.Id == id);
        }

        public TourExecution Create(TourExecution execution)
        {
            _dbSet.Add(execution);
            _dbContext.SaveChanges();
            return execution;
        }

        public TourExecution Update(TourExecution execution)
        {
            _dbContext.Entry(execution).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return execution;
        }

        public void Delete(long id)
        {
            var execution = _dbSet.FirstOrDefault(te => te.Id == id);
            if (execution != null)
            {
                _dbSet.Remove(execution);
                _dbContext.SaveChanges();
            }
        }

        public bool KeyPointExists(long keyPointId)
        {
            return _dbContext.KeyPoints.Any(kp => kp.Id == keyPointId);
        }

        public ICollection<KeyPoint> GetKeyPointsByTourId(long tourId)
        {
            var tour = _dbContext.Tour
                .Include(t => t.KeyPoints)
                .FirstOrDefault(t => t.Id == tourId);

            return tour?.KeyPoints.ToList() ?? new List<KeyPoint>();
        }

        public TourExecution? GetActiveByTourist(long touristId)
        {
            return _dbSet.FirstOrDefault(te => te.TouristId == touristId && te.Status == TourExecutionStatus.Active);
        }

        public TourExecution? GetByTourAndTourist(long touristId, long tourId)
        {
            return _dbSet.FirstOrDefault(te => te.TouristId == touristId && te.TourId == tourId);
        }

        public bool IsTourCompleted(long touristId, long tourId)
        {
            var execution = _dbSet.FirstOrDefault(te => te.TouristId == touristId && te.TourId == tourId);
            return execution != null && execution.Status == TourExecutionStatus.Completed;
        }

        public List<long> GetAllCompletedToursForUser(long touristId)
        {
            return _dbSet
                .Where(te => te.TouristId == touristId && te.Status == TourExecutionStatus.Completed)
                .Select(te => te.TourId)
                .ToList();
        }
    }
}
