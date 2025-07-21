using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/account")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        private readonly IPersonService _personService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;

        public AccountController(IAccountService accountService, IPersonService personService, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _accountService = accountService;
            _personService = personService;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
        }

        [HttpGet]
        public ActionResult<PagedResult<AccountDto>> GetAllAccount([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _accountService.GetPagedAccount(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPut("block")]
        public ActionResult<AccountDto> BlockUser([FromBody] AccountDto account)
        {
            var result = _accountService.BlockUser(account);
            return CreateResponse(result);
        }

        [HttpGet("wallet/{id:int}")]
        public ActionResult<PersonDto> GetWallet(int id)
        {
            var result = _personService.Get(id);
            return CreateResponse(result);
        }

        [HttpPut("wallet/{id:int}")]
        public ActionResult<PersonDto> Update([FromBody] PersonDto person)
        {
            if (!string.IsNullOrEmpty(person.ImageBase64))
            {

                // Brisanje stare slike ako postoji
                if (!string.IsNullOrEmpty(person.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, person.ImageUrl);
                    _imageService.DeleteOldImage(oldImagePath);
                }


                // Konvertovanje slike iz base64 formata
                var imageData = Convert.FromBase64String(person.ImageBase64.Split(',')[1]);

                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "person");

                person.ImageUrl = _imageService.SaveImage(folderPath, imageData, "person");
            }
            var result = _personService.Update(person);
            return CreateResponse(result);
        }
    }
}
