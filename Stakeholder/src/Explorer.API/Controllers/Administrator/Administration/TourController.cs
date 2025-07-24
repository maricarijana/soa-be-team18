//using Explorer.Tours.API.Dtos;
//using Explorer.Tours.API.Public.TourAuthoring;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Explorer.API.Controllers.Administrator.Administration
//{

//    [Authorize(Policy = "administratorPolicy")]
//    [Route("api/admin/tour")]
//    public class TourController :BaseApiController
//    {
//        private readonly ITourService _tourService;

//        public TourController(ITourService tourService)
//        {
//            _tourService = tourService;
//        }

//        [HttpDelete("{id}")]
//        public ActionResult DeleteTour(int id)
//        {
//            var result = _tourService.DeleteTour(id);

//            if (result.IsSuccess)
//            {
//                return NoContent();
//            }
//            else
//            {
//                return BadRequest(result.Errors);
//            }
//        }
//        [HttpGet("getById/{id:long}")]
//        public ActionResult<TourDto> GetById(long id)
//        {
//            var result = _tourService.GetById(id);
//            return CreateResponse(result);
//        }
//        [HttpGet("getByTourId/{id:long}")]
//        public ActionResult<TourDto> GetTourById(long id)
//        {
//            var result = _tourService.GetTourById(id);
//            return CreateResponse(result);
//        }

//    }
//}
