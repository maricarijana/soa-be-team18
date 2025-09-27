using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tours.API.Controllers;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;
using FluentResults;

namespace Tours.API.Controllers.Author
{
    //[Authorize(Policy = "authorPolicy")]
    [Route("api/tours")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpPost]
        public ActionResult<TourDto> Create([FromBody] TourDto tour)
        {
            var result = _tourService.Create(tour);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<List<TourDto>> GetAllByUserId(int id)
        {
            var result = _tourService.GetByUserId(id);
            return CreateResponse(result);
        }


       

        [HttpPut("archive/{tourId:int}")]
        public ActionResult<TourDto> Archive(long tourId)
        {
            var result = _tourService.Archive(tourId);
            return CreateResponse(result);
        }

        [HttpPut("publish/{tourId:int}")]
        public ActionResult<TourDto> Publish(long tourId)
        {
            var result = _tourService.Publish(tourId);
            return CreateResponse(result);
        }


        [HttpPut("reactivate/{tourId:int}")]
        public ActionResult<TourDto> Reactivate(long tourId)
        {
            var result = _tourService.Reactivate(tourId);
            return CreateResponse(result);
        }

        [HttpPut("updateDistance/{tourId:int}")]
        public ActionResult<TourDto> UpdateDistance(long tourId, [FromBody] double distance)
        {
            var result = _tourService.UpdateDistance(tourId, distance);
            return CreateResponse(result);
        }

        [HttpGet("getById/{id:long}")]
        public ActionResult<TourDto> GetById(long id)
        {
            var result = _tourService.GetById(id);
            return CreateResponse(result);
        }
        [HttpGet("getByTourId/{id:long}")]
        public ActionResult<TourDto> GetTourById(long id)
        {
            var result = _tourService.GetTourById(id);
            return CreateResponse(result);
        }

    }
}
