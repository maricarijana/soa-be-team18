using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Authentication
{
    public class BlockUserTests : BaseStakeholdersIntegrationTest
    {
        public BlockUserTests(StakeholdersTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Successfully_block_user()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var userToBeBlocked = new AccountDto
            {
                Id = -23,
                Username = "turista3@gmail.com",
                Password = "turista3",
                Email = "turista3@gmail.com",
                Role = "Tourist",
                IsActive = true,
            };

            var result = ((ObjectResult)controller.BlockUser(userToBeBlocked).Result)?.Value as AccountDto;

            result.ShouldNotBeNull();
            result.ShouldBeSameAs(userToBeBlocked);
            result.IsActive.ShouldBeFalse();
        }

        private static AccountController CreateController(IServiceScope scope)
        {
            return new AccountController(scope.ServiceProvider.GetRequiredService<IAccountService>(),
                scope.ServiceProvider.GetRequiredService<IPersonService>(), scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>(),
                scope.ServiceProvider.GetRequiredService<IImageService>());
        }
    }
}

