using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tours.Application.Public;

namespace Tours.API.Controllers.Tourist
{
    [Route("api/tourist/purchase")]
    public class PurchaseController: BaseApiController
    {
        private readonly IPurchaseService purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            this.purchaseService = purchaseService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] long userId)
        {
            var result = await purchaseService.CreateAsync(userId);
            if (result.IsFailed)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet("user/{userId:long}/tour/{tourId:int}/is-purchased")]
        public async Task<IActionResult> IsPurchased(long userId, int tourId)
        {
            var result = await purchaseService.IsPurchasedAsync(userId, tourId);
            if (result.IsFailed)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

    }
}
