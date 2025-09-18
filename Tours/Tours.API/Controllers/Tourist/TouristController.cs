using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tours.API.Controllers;
using Tours.Application.Public.Author;

namespace Tours.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/person/tourist")]
    public class TouristController : BaseApiController
    {
        //private readonly IPersonService _personService;
        private readonly ITourService _tourService;

        //public TouristController(IPersonService personService, ITourService tourService)
        //{
        //    _personService = personService;
        //    _tourService = tourService;
        //}
        public TouristController(ITourService tourService)
        {
            _tourService = tourService;
        }


        [HttpGet("tour/{tourId:int}")]
        public ActionResult GetTour(int tourId)
        {
            var result = _tourService.GetWithKeyPoints(tourId);
            return CreateResponse(result);
        }
        [HttpGet("getAuthor/{tourId:int}")]
        public ActionResult<long> GetAuthorId(int tourId)
        {
            var result = _tourService.GetTourById(tourId);
            if (result.IsFailed || result.Value == null)
            {
                return NotFound("Tour not found.");
            }

            // Pretpostavlja se da `UserId` predstavlja autora ture
            return Ok(result.Value.UserId);
        }
    }
}
