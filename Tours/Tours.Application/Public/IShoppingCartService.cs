using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Application.Dtos.Shopping;

namespace Tours.Application.Public
{
    public interface IShoppingCartService
    {
        Task<Result<ShoppingCartItemDto>> AddToCartAsync(ShoppingCartItemCreationDto itemCreationDto);
        Task<Result> RemoveFromCartAsync(long tourId, long userId);
        Task<ShoppingCartDto?> GetShoppingCartByUserIdAsync(long userId);
        Task ClearCartAsync(long userId);
    }
}
