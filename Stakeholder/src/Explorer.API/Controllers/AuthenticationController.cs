using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Route("api/users")]
public class AuthenticationController : BaseApiController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IImageService _imageService;

    public AuthenticationController(IAuthenticationService authenticationService,IImageService imageService, IWebHostEnvironment webHostEnvironment)
    {
        _authenticationService = authenticationService;
        _imageService = imageService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost]
    public ActionResult<AuthenticationTokensDto> RegisterTourist([FromBody] AccountRegistrationDto account)
    {
        if (!string.IsNullOrEmpty(account.ImageBase64))
        {
            // Konvertovanje base64 stringa u niz bajtova (posle uklanjanja prefiksa tipa podataka ako je potrebno)
            var imageData = Convert.FromBase64String(account.ImageBase64.Split(',')[1]);

            // Definišite putanju foldera za čuvanje slike
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "person");

            // Sačuvajte sliku koristeći pomoćnu servisnu metodu
            account.ProfilePicture = _imageService.SaveImage(folderPath, imageData, "person");
        }

        var result = _authenticationService.RegisterTourist(account);
        return CreateResponse(result);
    }

    [HttpPost("login")]
    public ActionResult<AuthenticationTokensDto> Login([FromBody] CredentialsDto credentials)
    {
        var result = _authenticationService.Login(credentials);
        return CreateResponse(result);
    }
}