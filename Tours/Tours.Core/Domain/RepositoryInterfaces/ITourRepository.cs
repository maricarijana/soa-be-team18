using Tours.Core.Domain;
using Tours.Core.UseCases;

namespace Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        List<Tour> GetToursByUserId(long userId);
        //List<Equipment> GetEquipment(long tourId);
        Tour GetSpecificTourByUser(long id, long userId);
        public Tour GetById(long id);
        public void Save();
        PagedResult<Tour> GetByKeyPoints(List<KeyPoint> keyPoints, int page, int pageSize);

        PagedResult<Tour> GetPublished(int page, int pageSize);        //izmenila sa pagedresult    -sad mzoes vratiti na to
        public Tour GetWithKeyPoints(int tourId);
        List<long> GetIdsByTag(TourTags tag);
        double SumOfTourLenght(List<long> completedTourIds);
        double FindMaxTourLength(List<long> completedTourIds);
    }
}
