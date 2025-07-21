using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Authentication
{
    [Collection("Sequential")]
    public class ClubInvitationCommandTests : BaseStakeholdersIntegrationTest
    {
        public ClubInvitationCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newEntity = new ClubInvitationDto
            {
                Id = 5,
                ClubId = -1,
                MemberId = 1,
                Status = ClubStatus.PROCESSING
             };

         // Act
         var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ClubInvitationDto;

         // Assert - Response
         result.ShouldNotBeNull();
         result.Id.ShouldNotBe(0);
         result.ClubId.ShouldBe(newEntity.ClubId);

                 // Assert - Database
                 var storedEntity = dbContext.ClubInvitations.FirstOrDefault(i => i.Id == newEntity.Id);
                    storedEntity.ShouldNotBeNull();
                 storedEntity.Id.ShouldBe(result.Id);
         }
        
           [Fact]
           public void Create_fails_invalid_data()
           {
               // Arrange
               using var scope = Factory.Services.CreateScope();
               var controller = CreateController(scope);
               var updatedEntity = new ClubInvitationDto
               {
                   Id = 0
               };

               // Act
               var result = (ObjectResult)controller.Create(updatedEntity).Result;

               // Assert
               result.ShouldNotBeNull();
               result.StatusCode.ShouldBe(400);
           }

        private static ClubInvitationController CreateController(IServiceScope scope)
        {
            return new ClubInvitationController(scope.ServiceProvider.GetRequiredService<IClubInvitationService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}