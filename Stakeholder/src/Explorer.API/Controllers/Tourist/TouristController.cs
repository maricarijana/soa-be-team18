//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Explorer.Tours.API.Public.TourAuthoring;
//using Explorer.Tours.Core.Domain;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Explorer.API.Controllers.Tourist
//{
//    [Authorize(Policy = "touristPolicy")]
//    [Route("api/person/tourist")]
//    public class TouristController :BaseApiController
//    {
//        private readonly IPersonService _personService;
//        private readonly ITourService _tourService;

//        public TouristController(IPersonService personService, ITourService tourService)
//        {
//            _personService = personService;
//            _tourService = tourService;
//        }

//        [HttpGet("{id:int}")]
//        public ActionResult Get(int id)
//        {
//            var result = _personService.Get(id);
//            return CreateResponse(result);
//        }

//        [HttpPut("{id:int}")]
//        public ActionResult Update([FromBody]PersonDto personDto)
//        {
//            var result = _personService.Update(personDto);
//            return CreateResponse(result);
//        }

//        [HttpGet("tour/{tourId:int}")]
//        public ActionResult GetTour(int tourId)
//        {
//            var result = _tourService.GetWithKeyPoints(tourId);
//            return CreateResponse(result);
//        }
//        [HttpGet("getAuthor/{tourId:int}")]
//        public ActionResult<long> GetAuthorId(int tourId)
//        {
//            var result = _tourService.GetTourById(tourId);
//            if (result.IsFailed || result.Value == null)
//            {
//                return NotFound("Tour not found.");
//            }

//            // Pretpostavlja se da `UserId` predstavlja autora ture
//            return Ok(result.Value.UserId);
//        }
//    }
//}
