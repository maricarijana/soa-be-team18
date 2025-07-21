using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Http.Features;
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
    public class ClubJoinRequestQueryTests : BaseStakeholdersIntegrationTest
    {
        public ClubJoinRequestQueryTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<ClubJoinRequestDto>;

            //Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(4);
            result.TotalCount.ShouldBe(4);  
        }

        [Fact]
        public void Retrieves_requests_for_club_members()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetClubMembers(0, 0, 2).Result);
            var responseBody = result.Value as List<ClubJoinRequestDto>;
            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);


            responseBody.ShouldNotBeNull();
            responseBody.Count.ShouldBe(1);

        }



        private static ClubJoinRequestController CreateController(IServiceScope scope)
        {
            return new ClubJoinRequestController(scope.ServiceProvider.GetRequiredService<IClubJoinRequestService>())
            {
                ControllerContext = BuildContext("-1")
            };

        }
    }
}
