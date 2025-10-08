using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tours.API.Controllers;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using Tours.Core.UseCases;
using FluentResults;

namespace Tours.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tour/tourOverview")]
    public class TourOverviewController : BaseApiController
    {
        private readonly ITourOverviewService _tourOverviewService;

        public TourOverviewController(ITourOverviewService tourOverviewService)
        {
            _tourOverviewService = tourOverviewService;
        }

        //[HttpGet]
        //public ActionResult<List<TourOverviewDto>> GetAllWithoutreviews([FromQuery] int page, [FromQuery] int pageSize)
        //{
        //    var result = _tourOverviewService.GetAllWithoutReviews(page, pageSize);
        //    return CreateResponse(result);
        //}

        [HttpGet("byId/{id:int}")]
        public ActionResult<PagedResult<TourOverviewDto>> GetById(int id)
        {
            var result = _tourOverviewService.GetById(id);
            return CreateResponse(result);
        }



        //[HttpGet("average/{id:int}")]
        //public ActionResult<TourOverviewDto> GetAveragerating(long id)
        //{
        //    var result = _tourOverviewService.GetAverageRating(id);
        //    return CreateResponse(result);
        //}

        [HttpGet("search/{longitude:double};{latitude:double};{distance:int}")]
        public ActionResult<PagedResult<TourOverviewDto>> GetByCoordinated(double latitude, double longitude, int distance, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourOverviewService.GetByCoordinated(latitude, longitude, distance, page, pageSize);
            return CreateResponse(result);
        }

    }
}
