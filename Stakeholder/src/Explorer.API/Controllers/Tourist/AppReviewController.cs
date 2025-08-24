using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/appReview")]
    public class AppReviewController: BaseApiController
    {
        private readonly IAppReviewService _appReviewService;

        public AppReviewController(IAppReviewService appReviewService)
        {
            _appReviewService = appReviewService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<AppReviewDto> Get(int id)
        {
            var result = _appReviewService.Get(id);
            return CreateResponse(result);
        }


        [HttpPost]
        public ActionResult<AppReviewDto> Create([FromBody] AppReviewDto appReview)
        {
            var result = _appReviewService.Create(appReview);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<AppReviewDto> Update([FromBody] AppReviewDto appReview)
        {
            var result = _appReviewService.Update(appReview);
            return CreateResponse(result);
        }
    }
}
