using Microsoft.AspNetCore.Mvc;
using Tours.Application.Dtos;
using Tours.Application.Public.Author;

namespace Tours.API.Controllers.Tourist
{
    [ApiController]
    [Route("api/tour-execution")]
    public class TourExecutionController : ControllerBase
    {
        private readonly ITourExecutionService _executionService;

        public TourExecutionController(ITourExecutionService executionService)
        {
            _executionService = executionService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TourExecutionDto dto)
        {
            var result = _executionService.Create(dto);
            if (result.IsFailed) return BadRequest(result.Errors);
            return Ok(result.Value);
        }

        [HttpPut("complete/{executionId:long}")]
        public IActionResult CompleteTourExecution(long executionId)
        {
            var result = _executionService.CompleteTourExecution(executionId);
            if (result.IsFailed) return NotFound(result.Errors);
            return Ok(result.Value);
        }

        [HttpPut("abandon/{executionId:long}")]
        public IActionResult AbandonTourExecution(long executionId)
        {
            var result = _executionService.AbandonTourExecution(executionId);
            if (result.IsFailed) return NotFound(result.Errors);
            return Ok(result.Value);
        }

        [HttpPut("complete-keypoint/{executionId:long}/{keyPointId:long}")]
        public IActionResult CompleteKeyPoint(long executionId, long keyPointId)
        {
            var result = _executionService.CompleteKeyPoint(executionId, keyPointId);
            if (result.IsFailed)
                return Conflict(new { message = result.Errors.First().Message });
            return Ok(result.Value);
        }

        [HttpPut("update-last-activity/{executionId:long}")]
        public IActionResult UpdateLastActivity(long executionId)
        {
            try
            {
                _executionService.UpdateLastActivity(executionId);
                return Ok(new { message = "Last activity updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating last activity.", details = ex.Message });
            }
        }

        [HttpGet("by-tour-and-tourist/{touristId:long}/{tourId:long}")]
        public IActionResult GetByTourAndTourist(long touristId, long tourId)
        {
            var result = _executionService.GetByTourAndTouristId(touristId, tourId);
            if (result == null || result.IsFailed) return NotFound("Tour execution not found.");
            return Ok(result.Value);
        }

        [HttpGet("{tourId:long}/keypoints")]
        public IActionResult GetKeyPointsForTour(long tourId)
        {
            var keyPoints = _executionService.GetKeyPointsForTour(tourId);
            return Ok(keyPoints);
        }

        [HttpGet("active/{touristId:long}")]
        public IActionResult GetActiveTour(long touristId)
        {
            var result = _executionService.GetActiveTourByTouristId(touristId);
            if (result.IsFailed) return NotFound(result.Errors);
            return Ok(result.Value);
        }
    }
}
