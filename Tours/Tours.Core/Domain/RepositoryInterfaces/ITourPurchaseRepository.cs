using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Core.Domain.Shopping;

namespace Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourPurchaseRepository
    {
        void Create(IEnumerable<TourPurchaseToken> tokens);
        Task SaveChangesAsync();
        Task<TourPurchaseToken?> GetByIdAsync(long id);
        Task<bool> IsPurchasedAsync(long userId, int tourId);

    }
}
