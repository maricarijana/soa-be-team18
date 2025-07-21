using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Database;
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
    [Collection("Sequential")]
    public class AddMemberTest:BaseStakeholdersIntegrationTest
    {
        public AddMemberTest(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void AddMember_AddsUserId_ToList()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var initialClub = dbContext.Clubs.FirstOrDefault(c => c.Id == -1);
            var initialMemberCount = initialClub.UserIds.Count;

            //Act
            var result = controller.AddMember(33, -1, -1);


            //Assert - Response
            result.ShouldNotBeNull();

            //Assert - Database
            var storedEntity = dbContext.Clubs.FirstOrDefault(c => c.Id == -1);
            storedEntity.ShouldNotBeNull();
            storedEntity.UserIds.Count.ShouldBe(initialMemberCount+1);

        }


        private static ClubController CreateController(IServiceScope scope)
        {
            return new ClubController(scope.ServiceProvider.GetRequiredService<IClubService>(), scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>(), scope.ServiceProvider.GetRequiredService<IImageService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}
