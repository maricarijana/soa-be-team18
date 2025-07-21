using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Core.UseCases;
using FluentResults;

namespace Explorer.API.Controllers
{
    [Route("api/user")]
    public class UserController: BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<UserDto> GetUsername(long id)
        {
            var username = _userService.GetUsername(id);
            return CreateResponse(username);
        }
        [HttpGet("{userId}/person-id")]
        public ActionResult<long> GetPersonId(long userId)
        {
            try
            {
                var personId = _userService.GetPersonId(userId);
                return Ok(personId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
        }
    }
}


