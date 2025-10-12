using AutoMapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tours.Application.Dtos.Shopping;
using Tours.Application.Public;
using Tours.Core.Domain.RepositoryInterfaces;
using Tours.Core.Domain.Shopping;

namespace Tours.Core.UseCases
{
    public class PurchaseService : IPurchaseService
    {
        private readonly ITourPurchaseRepository purchaseRepository;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IMapper mapper;

        public PurchaseService(
            ITourPurchaseRepository purchaseRepository,
            IShoppingCartService shoppingCartService,
            ITourRepository tourRepository,
            IMapper mapper)
        {
            this.purchaseRepository = purchaseRepository;
            this.shoppingCartService = shoppingCartService;
            this.mapper = mapper;
        }

        public async Task<Result<IEnumerable<TourPurchaseTokenDto>>> CreateAsync(long userId)
        {
            var cartDto = await shoppingCartService.GetShoppingCartByUserIdAsync(userId);
            if (cartDto is null || !cartDto.Items.Any())
                return Result.Fail("Shopping cart is empty");

            CvtCartItemDtosToTokens(cartDto.Items, userId, out var tokens);

            purchaseRepository.Create(tokens);
            await shoppingCartService.ClearCartAsync(userId);
            await purchaseRepository.SaveChangesAsync();

            return Result.Ok(mapper.Map<IEnumerable<TourPurchaseTokenDto>>(tokens));
        }

        private void CvtCartItemDtosToTokens(
            IEnumerable<ShoppingCartItemDto> items,
            long userId,
            out ICollection<TourPurchaseToken> tokens)
        {
            tokens = new List<TourPurchaseToken>();

            foreach (var item in items)
            {
                tokens.Add(new TourPurchaseToken
                {
                    TourId = item.TourId,
                    UserId = userId,
                });
            }
        }
        public async Task<Result<bool>> IsPurchasedAsync(long userId, int tourId)
        {
            var exists = await purchaseRepository.IsPurchasedAsync(userId, tourId);
            return Result.Ok(exists);
        }

    }
}
