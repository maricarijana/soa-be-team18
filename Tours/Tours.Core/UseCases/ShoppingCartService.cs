using AutoMapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Application.Dtos.Shopping;
using Tours.Application.Public;
using Tours.Application.Public.Author;
using Tours.Core.Domain.RepositoryInterfaces;
using Tours.Core.Domain.Shopping;


namespace Tours.Core.UseCases
{
    public class ShoppingCartService:IShoppingCartService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly ITourService tourService;
        private readonly IMapper mapper;

        public ShoppingCartService(
            IShoppingCartRepository shoppingCartRepository,
            ITourService tourService,
            IMapper mapper)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.tourService = tourService;
            this.mapper = mapper;
        }

        public async Task<Result<ShoppingCartItemDto>> AddToCartAsync(ShoppingCartItemCreationDto itemCreationDto)
        {
            var cart =
                await shoppingCartRepository.GetByUserIdAsync(itemCreationDto.UserId) ??
                shoppingCartRepository.Create(new ShoppingCart { UserId = itemCreationDto.UserId });

            // 🔹 Pre-check da li tura već postoji
            if (cart.Items.Any(i => i.TourId == itemCreationDto.TourId))
            {
                var existingItem = cart.Items.First(i => i.TourId == itemCreationDto.TourId);
                return Result.Ok(mapper.Map<ShoppingCartItemDto>(existingItem));
            }

            var tourResult = tourService.GetTourById(itemCreationDto.TourId);
            if (tourResult.IsFailed)
                return Result.Fail(tourResult.Errors);

            var tour = tourResult.Value;

            if (tour.Status == Application.Dtos.TourStatus.Archived)
                return Result.Fail("Cannot add archived tour to cart");

            var item = mapper.Map<ShoppingCartItem>(itemCreationDto);

            try
            {
                cart.AddToCart(item);
                await shoppingCartRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var msg = string.IsNullOrWhiteSpace(e.Message) ? "Unknown error occurred" : e.Message;
                return Result.Fail(msg);
            }

            return Result.Ok(mapper.Map<ShoppingCartItemDto>(item));
        }


        public async Task<Result> RemoveFromCartAsync(long tourId, long userId)
        {
            var cart = await shoppingCartRepository.GetByUserIdAsync(userId);
            if (cart is null)
                return Result.Fail("Shopping cart not found");

            try
            {
                cart.RemoveFromCart(tourId);
                await shoppingCartRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }

            return Result.Ok();
        }

        public async Task<ShoppingCartDto?> GetShoppingCartByUserIdAsync(long userId)
        {
            var cart = await shoppingCartRepository.GetByUserIdAsync(userId);
            return cart is null
                ? null
                : mapper.Map<ShoppingCartDto>(cart);
        }

        public async Task ClearCartAsync(long userId)
        {
            var cart = await shoppingCartRepository.GetByUserIdAsync(userId);
            _ = cart ?? throw new InvalidOperationException("Shopping cart not found");

            cart.EmptyCart();
        }
    }
}
