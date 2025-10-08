using FluentResults;
using Tours.Application.Dtos;

namespace Tours.Application.Public.Author
{
    public interface ITourOverviewService
    {
        // BITNO:promenila sam sve u ovoj metodi sa PagedResult na List
        //Result<List<TourOverviewDto>> GetAllWithoutReviews(int page, int pageSize);   
        Result<TourOverviewDto> GetById(int id);
        //Result<TourOverviewDto> GetAverageRating(long tourId);
        Result<List<TourOverviewDto>> GetByCoordinated(double latitude, double longitude, int distance, int page, int pageSize);
    }
}
