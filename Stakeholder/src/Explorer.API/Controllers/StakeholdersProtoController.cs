using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Grpc.Core;
using GrpcServiceTranscoding;
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


        public StakeholdersProtoController(
            ILogger<StakeholdersProtoController> logger,
            IUserService userService,
            IAccountService accountService,
             IAuthenticationService authenticationService)

        {
            _logger = logger;
            _userService = userService;
            _accountService = accountService;
            _authenticationService = authenticationService;

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




    }

}
