//using Explorer.API.Controllers.Administrator.Administration;
//using Explorer.BuildingBlocks.Core.UseCases;
//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Explorer.Tours.API.Dtos;
//using Explorer.Tours.API.Public.Administration;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Explorer.Stakeholders.Tests.Integration.Authentication
//{
//    [Collection("Sequential")]
//    public class AccountQueryTests : BaseStakeholdersIntegrationTest
//    {
//        public AccountQueryTests(StakeholdersTestFactory factory) : base(factory)
//        {
//        }

//        [Fact]
//        public void Retrieves_all()
//        {
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);

//            var result = ((ObjectResult)controller.GetAllAccount(0, 0).Result)?.Value as PagedResult<AccountDto>;

//            result.ShouldNotBeNull();
//            result.Results.Count.ShouldBe(9);
//            result.TotalCount.ShouldBe(9);
//        }

//        private static AccountController CreateController(IServiceScope scope)
//        {
//            return new AccountController
//                (scope.ServiceProvider.GetRequiredService<IAccountService>(),
//                scope.ServiceProvider.GetRequiredService<IPersonService>(), scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>(),
//                scope.ServiceProvider.GetRequiredService<IImageService>());
//        }
//    }
//}

