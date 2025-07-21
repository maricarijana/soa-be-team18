using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;


namespace Explorer.Stakeholders.Tests.Integration.Authentication
{
    [Collection("Sequential")]
    public class NotificationsQueryTests : BaseStakeholdersIntegrationTest
    {
        public NotificationsQueryTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all_notifications_for_user()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            int userId = -11; // Postavi odgovarajući ID korisnika
            int page = 0;
            int pageSize = 10;

            // Act
            var result = ((ObjectResult)controller.GetAllByUser(userId, page, pageSize).Result)?.Value as PagedResult<NotificationDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
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