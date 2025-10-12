using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceTranscoding;
using Tours.Application.Dtos.Shopping;
using Tours.Application.Public;

namespace Tours.API.Controllers
{
    public class ShoppingCartProtoController : ShoppingCartService.ShoppingCartServiceBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IPurchaseService _purchaseService;

        public ShoppingCartProtoController(
            IShoppingCartService shoppingCartService,
            IPurchaseService purchaseService)
        {
            _shoppingCartService = shoppingCartService;
            _purchaseService = purchaseService;
        }

        public override async Task<ShoppingCartItemResponse> AddToCart(ShoppingCartItemCreationRequest request, ServerCallContext context)
        {
            var dto = new ShoppingCartItemCreationDto
            {
                UserId = request.UserId,
                TourId = request.TourId,
                TourName = request.TourName,
                Price = (decimal)request.Price
            };

            var result = await _shoppingCartService.AddToCartAsync(dto);
            if (result.IsFailed || result.Value == null)
                throw new RpcException(new Status(StatusCode.Internal, "Failed to add item to cart."));

            return new ShoppingCartItemResponse
            {
                Id = result.Value.Id,
                ShoppingCartId = result.Value.ShoppingCartId,
                TourId = result.Value.TourId,
                TourName = result.Value.TourName,
                Price = (double)result.Value.Price
            };
        }

        public override async Task<ShoppingCartResponse> GetShoppingCart(GetShoppingCartRequest request, ServerCallContext context)
        {
            var cart = await _shoppingCartService.GetShoppingCartByUserIdAsync(request.UserId);
            if (cart == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Shopping cart not found."));

            var response = new ShoppingCartResponse
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = (double)cart.TotalPrice
            };
            response.Items.AddRange(cart.Items.Select(i => new ShoppingCartItemResponse
            {
                Id = i.Id,
                ShoppingCartId = i.ShoppingCartId,
                TourId = i.TourId,
                TourName = i.TourName,
                Price = (double)i.Price
            }));

            return response;
        }

        public override async Task<Empty> RemoveFromCart(RemoveFromCartRequest request, ServerCallContext context)
        {
            var result = await _shoppingCartService.RemoveFromCartAsync(request.TourId, request.UserId);
            if (result.IsFailed)
                throw new RpcException(new Status(StatusCode.Internal, "Failed to remove from cart."));
            return new Empty();
        }
        public override async Task<PurchaseCartResponse> PurchaseCart(PurchaseCartRequest request, ServerCallContext context)
        {
            var result = await _purchaseService.CreateAsync(request.UserId);
            if (result.IsFailed)
                throw new RpcException(new Status(StatusCode.Internal, "Purchase failed."));

            var response = new PurchaseCartResponse();
            response.PurchasedTourIds.AddRange(result.Value.Select(t => (long)t.TourId)); // cast u long ako treba
            return response;
        }


        public override async Task<IsPurchasedResponse> IsPurchased(IsPurchasedRequest request, ServerCallContext context)
        {
            var result = await _purchaseService.IsPurchasedAsync(request.UserId, (int)request.TourId);
            return new IsPurchasedResponse { Purchased = result.Value };
        }
    }
}

