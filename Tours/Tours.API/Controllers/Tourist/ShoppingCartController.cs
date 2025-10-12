using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tours.Application.Dtos.Shopping;
using Tours.Application.Public;

namespace Tours.API.Controllers.Tourist
{
    [Route("api/tourist/shopping-cart")]
    public class ShoppingCartController : BaseApiController
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(ShoppingCartItemCreationDto itemCreationDto)
        {
            var result = await shoppingCartService.AddToCartAsync(itemCreationDto);
            if (result.IsFailed)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet("user/{userId:long}")]
        public async Task<IActionResult> GetShoppingCartByUserId([FromRoute] long userId)
        {
            var cart = await shoppingCartService.GetShoppingCartByUserIdAsync(userId);

            if (cart is null)
                return NotFound($"Shopping cart for user with ID {userId} not found.");

            return Ok(cart);
        }


        [HttpDelete("user/{userId:long}/tour/{tourId:long}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] long userId, [FromRoute] long tourId)
        {
            var result = await shoppingCartService.RemoveFromCartAsync(tourId, userId);
            if (result.IsFailed)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
