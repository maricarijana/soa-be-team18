using Tours.Core.Domain;
using Tours.Core.Domain.RepositoryInterfaces;
using Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Tours.Core.UseCases;


namespace Tours.Infrastructure.Database.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly ToursContext _dbContext;
        private readonly DbSet<Tour> _dbSet;

        public TourRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Tour>();
        }

        public Tour GetById(long id)
        {
            var tour = _dbSet.FirstOrDefault(t => t.Id == id);
            if (tour == null)
            {
                throw new ArgumentException("Tour not found.");
            }
            return tour;
        }

        public PagedResult<Tour> GetByKeyPoints(List<KeyPoint> keyPoints, int page, int pageSize)
        {
            var keyPointIdsToMatch = keyPoints.Select(kp => kp.Id).ToHashSet();

            var res = _dbContext.Tour.Include(t => t.KeyPoints)
                .Where(tour => tour.KeyPoints.Any(kp => keyPointIdsToMatch.Contains(kp.Id)))
                .Skip((page - 1) * pageSize);

            if (pageSize > 0)
            {
                res = res.Take(pageSize);
            }

            var ret = res.ToList();

            return new PagedResult<Tour>(ret, ret.Count());
        }




        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public List<Tour> GetToursByUserId(long userId)
        {

            return _dbContext.Tour.Include(t => t.KeyPoints)
                         .Where(t => t.UserId == userId)
                         .ToList();
        }


        public Tour GetSpecificTourByUser(long tourId, long userId)
        {
            return _dbContext.Tour.SingleOrDefault(t => t.Id == tourId && t.UserId == userId);

        }

        public PagedResult<Tour> GetPublished(int page, int pageSize)
        {
            var query = _dbContext.Tour
                .Include(t => t.KeyPoints)
                .Where(t => t.Status == TourStatus.Published)
                .Skip((page - 1) * pageSize);

            if (pageSize > 0)
            {
                query = query.Take(pageSize);
            }

            var ret = query.ToList();

            return new PagedResult<Tour>(ret, ret.Count());
        }

       

        public Tour GetWithKeyPoints(int tourId)
        {
            var tour = _dbSet.Include(t => t.KeyPoints).FirstOrDefault(t => t.Id == tourId);
            if (tour == null)
            {
                throw new ArgumentException("Tour not found.");
            }
            return tour;
        }

        public List<long> GetIdsByTag(TourTags tag)
        {
            return _dbSet
        .Where(tour => tour.Tags.Contains(tag))
        .Select(tour => tour.Id)
        .ToList();
        }

        public double SumOfTourLenght(List<long> completedTourIds)
        {
            if (completedTourIds == null || !completedTourIds.Any())
                return 0;

            var totalLength = _dbContext.Tour
                .Where(t => completedTourIds.Contains(t.Id)) // Filter tours by the given IDs
                .Sum(t => t.LengthInKm); // Sum the LengthInKm of the filtered tours

            return totalLength;
        }

        public double FindMaxTourLength(List<long> completedTourIds)
        {
            if (completedTourIds == null || completedTourIds.Count < 2)
                return 0;

            // Fetch the first KeyPoint for each tour in completedTourIds
            var firstKeyPoints = _dbContext.Tour
                .Where(t => completedTourIds.Contains(t.Id))
                .Select(t => new
                {
                    t.Id,
                    FirstKeyPoint = t.KeyPoints.FirstOrDefault() // Assuming KeyPoints is a navigational property
                })
                .Where(t => t.FirstKeyPoint != null) // Ensure there's a valid first KeyPoint
                .Select(t => new
                {
                    t.Id,
                    t.FirstKeyPoint.Latitude,
                    t.FirstKeyPoint.Longitude
                })
                .ToList();

            if (firstKeyPoints.Count < 2)
                return 0;

            double maxDistance = 0;

            // Calculate pairwise distances
            for (int i = 0; i < firstKeyPoints.Count; i++)
            {
                for (int j = i + 1; j < firstKeyPoints.Count; j++)
                {
                    double distance = CalculateDistance(
                        firstKeyPoints[i].Latitude, firstKeyPoints[i].Longitude,
                        firstKeyPoints[j].Latitude, firstKeyPoints[j].Longitude
                    );
                    maxDistance = Math.Max(maxDistance, distance);
                }
            }

            return maxDistance;
        }

        // Helper method to calculate the distance between two geographic coordinates (Haversine formula)
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusKm = 6371.0;

            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        // Helper method to convert degrees to radians
        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

    }
}
