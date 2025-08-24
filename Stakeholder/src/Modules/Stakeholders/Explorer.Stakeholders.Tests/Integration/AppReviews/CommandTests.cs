//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Explorer.Stakeholders.Core.Domain;
//using Explorer.Stakeholders.Infrastructure.Database;
//using Explorer.Tours.API.Dtos;
//using Explorer.Tours.Infrastructure.Database;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Shouldly;

//namespace Explorer.Stakeholders.Tests.Integration.AppReviews
//{
//    [Collection("Sequential")]
//    public class CommandTests: BaseStakeholdersIntegrationTest
//    {
//        public CommandTests(StakeholdersTestFactory factory) : base(factory) { }

//        [Fact]
//        public void Creates()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
//            var newEntity = new AppReviewDto
//            {
//                Id = -3,
//                UserId = -3,
//                Grade = 3,
//                Comment = "onako",
//                CreationTime = DateTime.UtcNow,
//            };

//            // Act
//            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as AppReviewDto;

//            // Assert - Response
//            result.ShouldNotBeNull();
//            result.Id.ShouldNotBe(0);
//            result.Id.ShouldBe(newEntity.Id);
//            result.UserId.ShouldBe(newEntity.UserId);
//            result.Grade.ShouldBe(newEntity.Grade);
//            result.Comment.ShouldBe(newEntity.Comment);
//            result.CreationTime.ShouldBe(newEntity.CreationTime);

//            // Assert - Database
//            var storedEntity = dbContext.AppReviews.FirstOrDefault(i => i.UserId == newEntity.UserId);
//            storedEntity.ShouldNotBeNull();
//            storedEntity.Id.ShouldBe(result.Id);
//        }


//        [Fact]
//        public void Updates()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
//            var updatedEntity = new AppReviewDto
//            {
//                Id = -1,
//                UserId = -1,
//                Grade = 3,
//                Comment = "onako",
//                CreationTime = DateTime.UtcNow,
//            };

//            // Act
//            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as AppReviewDto;

//            // Assert - Response
//            result.ShouldNotBeNull();
//            result.Id.ShouldBe(-1);
//            result.UserId.ShouldBe(updatedEntity.UserId);
//            result.Grade.ShouldBe(updatedEntity.Grade);
//            result.Comment.ShouldBe(updatedEntity.Comment);

//            // Assert - Database
//            var storedEntity = dbContext.AppReviews.FirstOrDefault(i => i.UserId == -1);
//            storedEntity.ShouldNotBeNull();
//            storedEntity.Comment.ShouldBe(updatedEntity.Comment);
//        }

//        private static Explorer.API.Controllers.Tourist.AppReviewController CreateController(IServiceScope scope)
//        {
//            return new Explorer.API.Controllers.Tourist.AppReviewController(scope.ServiceProvider.GetRequiredService<IAppReviewService>())
//            {
//                ControllerContext = BuildContext("-1")
//            };
//        }
//    }
//}
