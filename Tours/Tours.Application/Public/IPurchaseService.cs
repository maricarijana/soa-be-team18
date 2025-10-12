using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Application.Dtos.Shopping;

namespace Tours.Application.Public
{
    public interface IPurchaseService
    {
        Task<Result<IEnumerable<TourPurchaseTokenDto>>> CreateAsync(long userId);
        Task<Result<bool>> IsPurchasedAsync(long userId, int tourId);

    }
}
