using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Grpc.Core;
using GrpcServiceTranscoding;
using System.Buffers.Text;

//using Microsoft.AspNetCore.Authentication;
using System.Net;

namespace Explorer.API.Controllers
{
    public class StakeholdersProtoController : StakeholdersService.StakeholdersServiceBase
    {
        private readonly ILogger<StakeholdersProtoController> _logger;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPersonService _personService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;



        public StakeholdersProtoController(
            ILogger<StakeholdersProtoController> logger,
            IUserService userService,
            IAccountService accountService,
            IAuthenticationService authenticationService,
            IPersonService personService,
            IWebHostEnvironment webHostEnvironment,
            IImageService imageService)

        {
            _logger = logger;
            _userService = userService;
            _accountService = accountService;
            _authenticationService = authenticationService;
            _personService = personService;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;

        }

        public override Task<User> Register(AccountRegistration request, ServerCallContext context)
        {
            _logger.LogInformation("Register called for username {Username}", request.Username);

            var dto = new AccountRegistrationDto
            {
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                ProfilePicture = request.ProfilePicture,
                Biography = request.Biography,
                Motto = request.Motto,
                Wallet = (decimal)request.Wallet,
                ImageBase64 = request.ImageBase64,
                Role = request.Role
            };

            var result = _userService.RegisterWithoutAuth(dto);

            return Task.FromResult(new User
            {
                Id = result.Value.Id,
                Username = result.Value.Username,
                IsActive = result.Value.IsActive
            });
        }

        public override Task<AccountList> GetAllAccounts(PagedRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetAllAccounts called: page {Page}, size {Size}", request.Page, request.PageSize);

            var result = _accountService.GetPagedAccount(request.Page, request.PageSize);

            var accounts = result.Value.Results.Select(a => new Account
            {
                Id = a.Id,
                Username = a.Username,
                Email = a.Email,
                Role = a.Role,
                IsActive = a.IsActive
            });

            var response = new AccountList();
            response.Accounts.AddRange(accounts);

            return Task.FromResult(response);
        }

        public override Task<AuthenticationTokens> Login(Credentials request, ServerCallContext context)
        {
            _logger.LogInformation("Login called for username {Username}", request.Username);

            var dto = new CredentialsDto
            {
                Username = request.Username,
                Password = request.Password
            };

            var result = _authenticationService.Login(dto);

            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, result.Errors.FirstOrDefault()?.Message ?? "Login failed"));
            }

            return Task.FromResult(new AuthenticationTokens
            {
                AccessToken = result.Value.AccessToken
            });
        }


        public override Task<Account> BlockUser(BlockRequest request, ServerCallContext context)
        {
            _logger.LogInformation("BlockUser called for account {AccountId}", request.AccountId);

            var result = _accountService.BlockUser(request.AccountId);

            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, result.Errors.FirstOrDefault()?.Message ?? "User not found"));
            }

            return Task.FromResult(new Account
            {
                Id = result.Value.Id,
                Username = result.Value.Username ?? string.Empty,
                Email = result.Value.Email ?? string.Empty,  
                Role = result.Value.Role ?? string.Empty,
                IsActive = result.Value.IsActive
            });
        }
        public override Task<Person> GetPerson(PersonRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetPerson called for ID {Id}", request.Id);

            //Provera uloge
            //var role = context.GetHttpContext()?.User.FindFirst("role")?.Value;
            //if (roleClaim == "0") roleClaim = "admin";
            //else if (roleClaim == "1") roleClaim = "author";
            //else if (roleClaim == "2") roleClaim = "tourist";
            //if (role != "Tourist" && role != "Author")
            var httpContext = context.GetHttpContext();
            string? role =
                httpContext?.User.FindFirst("role")?.Value ??
                httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ??
                httpContext?.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            _logger.LogInformation("Extracted role: {Role}", role ?? "null");

            if (string.IsNullOrEmpty(role))
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, "Access denied (no role found)."));
            }

            if (role.ToLower() != "tourist" && role.ToLower() != "author")
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, $"Access denied. Role = {role}"));
            }

            var result = _personService.Get((int)request.Id);
            if (!result.IsSuccess || result.Value == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found"));
            }

            var person = result.Value;
            //return Task.FromResult(new Person
            //{
            //    Id = person.Id,
            //    Name = person.Name ?? string.Empty,
            //    Surname = person.Surname ?? string.Empty,
            //    Biography = person.Biography ?? string.Empty,
            //    Motto = person.Motto ?? string.Empty,
            //    ImageUrl = person.ImageUrl ?? string.Empty
            //});
            return Task.FromResult(new Person
            {
                Id = person.Id,
                UserId = person.UserId,
                Name = person.Name ?? string.Empty,
                Surname = person.Surname ?? string.Empty,
                Email = person.Email ?? string.Empty,
                Biography = person.Biography ?? string.Empty,
                Motto = person.Motto ?? string.Empty,
                ImageUrl = person.ImageUrl ?? string.Empty
            });

        }
        public override Task<Person> UpdatePerson(PersonUpdateRequest request, ServerCallContext context)
        {
            _logger.LogInformation("UpdatePerson called for ID {Id}", request.Id);

            var httpContext = context.GetHttpContext();
            string? role =
                httpContext?.User.FindFirst("role")?.Value ??
                httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ??
                httpContext?.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            _logger.LogInformation("Extracted role: {Role}", role ?? "null");

            if (string.IsNullOrEmpty(role))
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, "Access denied (no role found)."));
            }

            if (role.ToLower() != "tourist" && role.ToLower() != "author")
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, $"Access denied. Role = {role}"));
            }

            //var personDto = new PersonDto
            //{
            //    Id = (int)request.Id,
            //    Name = request.Name,
            //    Surname = request.Surname,
            //    Biography = request.Biography,
            //    Motto = request.Motto,
            //    ImageBase64 = request.ImageBase64,
            //    ImageUrl = request.ImageUrl
            //};
            var personDto = new PersonDto
            {
                Id = (int)request.Id,
                UserId = request.UserId,
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Biography = request.Biography,
                Motto = request.Motto,
                ImageBase64 = request.ImageBase64,
                ImageUrl = request.ImageUrl
            };


            if (!string.IsNullOrEmpty(personDto.ImageBase64))
            {
                if (!string.IsNullOrEmpty(personDto.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, personDto.ImageUrl);
                    _imageService.DeleteOldImage(oldImagePath);
                }

                string base64 = personDto.ImageBase64;

                // Ako ima prefiks "data:image/png;base64,", izdvoji samo base64 deo
                if (base64.Contains(","))
                {
                    base64 = base64.Split(',')[1];
                }

                var imageData = Convert.FromBase64String(base64);
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "person");
                personDto.ImageUrl = _imageService.SaveImage(folderPath, imageData, "person");
                //var imageData = Convert.FromBase64String(personDto.ImageBase64.Split(',')[1]);
                //var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "person");
                //personDto.ImageUrl = _imageService.SaveImage(folderPath, imageData, "person");
            }

            var result = _personService.Update(personDto);
            //if (!result.IsSuccess || result.Value == null)
            //{
            //    throw new RpcException(new Status(StatusCode.Internal, "Failed to update person"));
            //}
            if (!result.IsSuccess || result.Value == null)
            {
                var errorMsg = result.Errors?.FirstOrDefault()?.Message ?? "Unknown error";
                _logger.LogError(" Failed to update person ID {Id}. Reason: {Reason}", request.Id, errorMsg);
                throw new RpcException(new Status(StatusCode.Internal, $"Failed to update person: {errorMsg}"));
            }


            var updated = result.Value;
            return Task.FromResult(new Person
            {
                Id = updated.Id,
                Name = updated.Name ?? string.Empty,
                Surname = updated.Surname ?? string.Empty,
                Biography = updated.Biography ?? string.Empty,
                Motto = updated.Motto ?? string.Empty,
                ImageUrl = updated.ImageUrl ?? string.Empty
            });
        }






    }

}
