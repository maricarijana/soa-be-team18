using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Core.UseCases;

namespace Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourReviewRepository
    {
        public PagedResult<TourReview> GetByTourId(long tourId, int page, int pageSize);
        public TourReview Get(long userId, long tourId);
    }
}
