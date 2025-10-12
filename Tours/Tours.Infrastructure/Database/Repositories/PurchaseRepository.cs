using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Core.Domain.RepositoryInterfaces;
using Tours.Core.Domain.Shopping;

namespace Tours.Infrastructure.Database.Repositories
{
    internal class PurchaseRepository : ITourPurchaseRepository
    {
        private readonly ToursContext context;
        public PurchaseRepository(ToursContext context)
        {
            this.context = context;
        }
        public void Create(IEnumerable<TourPurchaseToken> tokens)
        {
            context.TourPurchaseTokens.AddRangeAsync(tokens);
        }

        public async Task<TourPurchaseToken?> GetByIdAsync(long id)
        {
            return await context.TourPurchaseTokens.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        public async Task<bool> IsPurchasedAsync(long userId, int tourId)
        {
            return await context.TourPurchaseTokens
                .AnyAsync(p => p.UserId == userId && p.TourId == tourId);
        }

    }
}
