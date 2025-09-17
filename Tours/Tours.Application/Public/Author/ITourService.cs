using FluentResults;
using Tours.Application.Dtos;


namespace Tours.Application.Public.Author
{
    public interface ITourService
    {
        Result<TourDto> Create(TourDto dto);
        Result<List<TourDto>> GetByUserId(long userId);
        //Result<PagedResult<EquipmentDto>> GetEquipment(long tourId);
        Result<TourDto> Get(int id);
        public Result GetById(long id);
        Result UpdateDistance(long id, double distance);
        Result Archive(long id);
        Result Publish(long id);
        Result Reactivate(long id);
        Result DeleteTour(int id);
        Result<TourDto> GetWithKeyPoints(int tourId);
        Result<TourDto> GetTourById(long id);
        //Result<PagedResult<TourDto>> GetPublised();
    }
}
