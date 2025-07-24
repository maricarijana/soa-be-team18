using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Core.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Hosting;

namespace Explorer.API.Controllers
{
    [Route("api/user")]
    public class UserController: BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;

        public UserController(IUserService userService, IImageService imageService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
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

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<UserDto> Register([FromBody] AccountRegistrationDto account)
        {
            if (!string.IsNullOrEmpty(account.ImageBase64))
            {
                var imageData = Convert.FromBase64String(account.ImageBase64.Split(',')[1]);
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "person");

                account.ProfilePicture = _imageService.SaveImage(folderPath, imageData, "person");
            }

            var result = _userService.RegisterWithoutAuth(account);
            return CreateResponse(result);
        }

    }
}


