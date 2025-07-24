//using Explorer.BuildingBlocks.Core.UseCases;
//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//namespace Explorer.API.Controllers.Administrator.Administration
//{
//    [Authorize(Policy = "administratorPolicy")]
//    [Route("api/administration/appReview")]
//    public class AppReviewController : BaseApiController
//    {
//        private readonly IAppReviewService _appReviewService;

//        public AppReviewController(IAppReviewService appReviewService)
//        {
//            _appReviewService = appReviewService;
//        }

//        [HttpGet]
//        public ActionResult<PagedResult<AppReviewDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
//        {
//            var result = _appReviewService.GetPaged(page, pageSize);
//            return CreateResponse(result);
//        }
//    }
//}