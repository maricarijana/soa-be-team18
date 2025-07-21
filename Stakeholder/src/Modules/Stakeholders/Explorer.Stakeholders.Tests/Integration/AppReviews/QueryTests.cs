using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.AppReviews
{
    [Collection("Sequential")]
    public class QueryTests: BaseStakeholdersIntegrationTest
    {
        public QueryTests(StakeholdersTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Retrieves_all()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateAdminController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<AppReviewDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(2);
            result.TotalCount.ShouldBe(2);
        }

        [Fact]
        public void Retrieves_by_id()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateTouristController(scope);

            // Act
            var result = ((ObjectResult)controller.Get(-1).Result)?.Value as AppReviewDto;

            // Assert
            result.ShouldNotBeNull();
            result.UserId.ShouldBe(-1);
            result.Grade.ShouldBe(5);
        }

        private static Explorer.API.Controllers.Administrator.Administration.AppReviewController CreateAdminController(IServiceScope scope) 
        {
            return new Explorer.API.Controllers.Administrator.Administration.AppReviewController(scope.ServiceProvider.GetRequiredService<IAppReviewService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

        private static Explorer.API.Controllers.Tourist.AppReviewController CreateTouristController(IServiceScope scope)
        {
            return new Explorer.API.Controllers.Tourist.AppReviewController(scope.ServiceProvider.GetRequiredService<IAppReviewService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
