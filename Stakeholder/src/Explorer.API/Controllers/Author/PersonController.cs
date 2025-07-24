
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/person")]
    public class PersonController: BaseApiController
    {

        private readonly IPersonService _personService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PersonController(IPersonService personService, IWebHostEnvironment webHostEnvironment)
        {
            _personService = personService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPut("{id:int}")]
        public ActionResult<PersonDto> Update([FromBody] PersonDto person)
        {
            if (!string.IsNullOrEmpty(person.ImageBase64))
            {
                // Konvertovanje slike iz base64 formata
                var imageData = Convert.FromBase64String(person.ImageBase64.Split(',')[1]);
                var fileName = Guid.NewGuid() + ".png"; // ili format prema potrebi
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "person");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                // Brisanje stare slike ako postoji
                if (!string.IsNullOrEmpty(person.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, person.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Čuvanje nove slike
                System.IO.File.WriteAllBytes(filePath, imageData);
                person.ImageUrl = $"images/person/{fileName}";
            }
            var result = _personService.Update(person);
            return CreateResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            var result = _personService.Get(id);
            return CreateResponse(result);
        }
    }
}
