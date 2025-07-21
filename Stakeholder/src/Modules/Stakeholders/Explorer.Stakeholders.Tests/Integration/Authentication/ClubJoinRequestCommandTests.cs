using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ClubJoinRequestCommandTests : BaseStakeholdersIntegrationTest
    {
        public ClubJoinRequestCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            //Arange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newEntity = new ClubJoinRequestDto
            {
                Id = 5,
                ClubId = -1,
                UserId = 1,
                Status = JoinRequestStatus.PENDING
            };

            //Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ClubJoinRequestDto;

            //Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.ClubId.ShouldBe(newEntity.ClubId);

            //Assert - Database 
            var storedEntity = dbContext.ClubJoinRequests.FirstOrDefault(r => r.Id == newEntity.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var newEntity = new ClubJoinRequestDto
            {
                Id = 0
            };

            //Act
            var result = (ObjectResult)controller.Create(newEntity).Result;

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Updates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var updatedEntity = new ClubJoinRequestDto
            {
                Id = 1,
                UserId = 1,
                ClubId = 1,
                Status = JoinRequestStatus.ACCEPTED
            };

            //Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ClubJoinRequestDto;

            //Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.UserId.ShouldBe(updatedEntity.UserId);
            result.ClubId.ShouldBe(updatedEntity.ClubId);
            result.Status.ShouldBe(updatedEntity.Status);

            //Assert - Database
            var storedEntity = dbContext.ClubJoinRequests.FirstOrDefault(i => i.Id == 1);
            storedEntity.ShouldNotBeNull();
            storedEntity.UserId.ShouldBe(updatedEntity.UserId);

        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new ClubJoinRequestDto
            {
                Id = -1000,
                UserId = 1,
                ClubId = 1,
                Status = JoinRequestStatus.ACCEPTED
            };

            //Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Deletes()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            //Act
            var result = (OkResult)controller.Delete(4);
            //Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            //Assert - Database
            var storedCourse = dbContext.ClubJoinRequests.FirstOrDefault(i => i.Id == 4);
            storedCourse.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = (ObjectResult)controller.Delete(1000);

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
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
