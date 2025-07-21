using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Authentication
{
    [Collection("Sequential")]
    public class NotificationsCommandTests : BaseStakeholdersIntegrationTest
    {
        public NotificationsCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newEntity = new NotificationDto
            {
                Id = -5,
                Description = "New notification nesto",
                CreationTime = DateTime.Now.ToUniversalTime(),
                IsRead = false,
                UserId = 3,
                NotificationsType = NotificationDto.NotificationType.PROBLEM,
                ResourceId = 2
            };
            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as NotificationDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Description.ShouldBe(newEntity.Description);

            // Assert - Database
            var storedEntity = dbContext.Notification.FirstOrDefault(i => i.Description == newEntity.Description);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new NotificationDto
            {
                Description = "New notification nesto",
                CreationTime = DateTime.UtcNow.AddYears(10),
                IsRead = false,
                UserId = 3,
                NotificationsType = NotificationDto.NotificationType.PROBLEM,
                ResourceId = 2
            };

            // Act
            var result = (ObjectResult)controller.Create(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            //VALUES (-2, 'E imas novi problem', '2024-10-12 15:26:04.723', true, -11, 0, -3);
            var updatedEntity = new NotificationDto
            {
                Id = -2,
                Description = "Promenjeno mnogo dobro",
                CreationTime = DateTime.Now.ToUniversalTime(),
                IsRead = false,
                UserId = 11,
                NotificationsType = NotificationDto.NotificationType.PROBLEM,
                ResourceId = 3
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as NotificationDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-2);
            result.Description.ShouldBe(updatedEntity.Description);
            result.CreationTime.ShouldBe(updatedEntity.CreationTime);

            // Assert - Database
            var storedEntity = dbContext.Notification.FirstOrDefault(i => i.Description == "Promenjeno mnogo dobro");
            storedEntity.ShouldNotBeNull();
            storedEntity.Description.ShouldBe(updatedEntity.Description);
            var oldEntity = dbContext.Notification.FirstOrDefault(i => i.Description == "E imas novi problem");
            oldEntity.ShouldBeNull();
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new NotificationDto
            {
                Id = 158000,
                Description = "E imas novi problem",
                CreationTime = DateTime.Now.ToUniversalTime(),
                IsRead = false,
                UserId = 11,
                NotificationsType = NotificationDto.NotificationType.PROBLEM,
                ResourceId = 3
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }
        private static AuthorNotificationController CreateController(IServiceScope scope)
        {
            return new AuthorNotificationController(scope.ServiceProvider.GetRequiredService<INotificationService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}
