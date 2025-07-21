//using Explorer.API.Controllers.Tourist;
//using Explorer.BuildingBlocks.Core.UseCases;
//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Explorer.Stakeholders.Infrastructure.Database;
//using Explorer.Tours.API.Dtos;
//using Explorer.Tours.Infrastructure.Database;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Explorer.Stakeholders.Tests.Integration
//{
//    [Collection("Sequential")]
//    public class ProblemTest : BaseStakeholdersIntegrationTest
//    {
//        public ProblemTest(StakeholdersTestFactory factory) : base(factory) { }

//        [Fact]

//        public void Creates()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
//            var newEntity = new ProblemDTO
//            {
//                Id = -5,
//                UserId = -1,
//                TourId = 3,
//                Category = "ucitavanje",
//                Description = "Slika nije ucitana",
//                Priority = 5,
//                Time = DateTime.Parse("2024-10-16T14:00:00Z").ToUniversalTime(),
//                IsActive = true,
//                Deadline = 2
//            };

//            // Act
//            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ProblemDTO;

//            // Assert - Response
//            result.ShouldNotBeNull();
//            result.Id.ShouldNotBe(0);
//            result.Id.ShouldBe(newEntity.Id);

//            // Assert - Database
//            var storedEntity = dbContext.Problem.FirstOrDefault(i => i.Id == newEntity.Id);
//            storedEntity.ShouldNotBeNull();
//            storedEntity.Id.ShouldBe(result.Id);
//        }

//        [Fact]
//        public void Deletes()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

//            // Act
//            var result = (OkResult)controller.Delete(-4);

//            // Assert - Response
//            result.ShouldNotBeNull();
//            result.StatusCode.ShouldBe(200);

//            // Assert - Database
//            var storedCourse = dbContext.Problem.FirstOrDefault(i => i.Id == -4);
//            storedCourse.ShouldBeNull();
//        }

//        [Fact]

//        public void GetByUserId()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            long userId = 2;

//            // Act
//            var result = ((ObjectResult)controller.GetByTouristId(userId).Result)?.Value as List<ProblemDTO>;

//            // Assert - Database
//            result.ShouldNotBeNull();
//            result.Count.ShouldBe(2);
//            result[0].Id.ShouldBe(-2);
//            result[1].Id.ShouldBe(-3);
//        }

//        [Fact]
//        public void GetByTourId()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            long tourId = 2;

//            // Act
//            var result = ((ObjectResult)controller.GetByTourId(tourId).Result)?.Value as List<ProblemDTO>;

//            // Assert - Database
//            result.ShouldNotBeNull();
//            result.Count.ShouldBe(2);
//            result[0].Id.ShouldBe(-2);
//            result[1].Id.ShouldBe(-3);
//        }

//        [Fact]
//        public void AddProblemComment()
//        {
//            //Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
//            var newEntity = new ProblemCommentDto
//            {
//                ProblemId = -4,
//                UserId = -3,
//                Text = "tekst komentara",
//                TimeSent = DateTime.UtcNow
//            };
//            //Act
//            var result = ((ObjectResult)controller.PostComment(newEntity).Result)?.Value as ProblemDTO;
//            result.ShouldNotBeNull();
//            result.Id.ShouldBe(newEntity.ProblemId);
//        }


//        private static ProblemControllerTourist CreateController(IServiceScope scope)
//        {
//            return new ProblemControllerTourist(scope.ServiceProvider.GetRequiredService<IProblemService>());
//        }



//    }
//}
