
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Application.Dtos;

namespace Tours.Application.Public.Author
{
    public interface ITourReviewService
    {
        //Result<TourReviewDto> GetPaged(int page, int pageSize);
        Result<TourReviewDto> Get(long userId, long tourId);
        Result<TourReviewDto> Create(TourReviewDto tourReview);
        Result<TourReviewDto> Update(TourReviewDto tourReview);
        Result Delete(int id);
    }
}
