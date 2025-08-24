using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{

    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/person/")]
    public class PersonController: BaseApiController
    {
        private readonly IPersonService _personService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;
        public PersonController(IPersonService personService, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _personService = personService;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
        }

        [HttpPut("{id:int}")]
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


        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            var result = _personService.Get(id);
            return CreateResponse(result);
        }
    }
}

