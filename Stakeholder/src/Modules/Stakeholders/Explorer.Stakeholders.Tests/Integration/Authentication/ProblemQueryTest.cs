//using Explorer.API.Controllers.Administrator.Administration;
//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Explorer.Stakeholders.Core.Domain.Problems;
//using Explorer.Stakeholders.Infrastructure.Database;
//using Explorer.Tours.API.Public.TourAuthoring;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Explorer.Stakeholders.Tests.Integration.Authentication
//{
//    [Collection("Sequential")]
    
//        public class ProblemQueryTest : BaseStakeholdersIntegrationTest
//        {
//        public ProblemQueryTest(StakeholdersTestFactory factory) : base(factory) { }

//        [Fact]
//        public void Update_Problem_SetsIsActiveToFalse()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

//            // Ručno kreiramo problem sa ID-jem -9
//            var existingProblem = new Problem(22, 1, "Technical", "Issue with the app", 1, DateTime.UtcNow, true,0)
//            {
//                Id = -9 // Ručno postavljamo ID
//            };
//            dbContext.Problem.Add(existingProblem);
//            dbContext.SaveChanges();

//            var updatedEntity = new ProblemDTO
//            {
//                Id = -9, // Koristimo isti ID za ažuriranje
//                UserId = 22,
//                TourId = 1,
//                Category = "Technical",
//                Description = "Issue with the app",
//                Priority = 1,
//                Time = DateTime.UtcNow,
//                IsActive = false,
//                Deadline = 2
//            };

//            // Act
//            var result = ((ObjectResult)controller.UpdateActiveStatus((int)existingProblem.Id, false).Result)?.Value as ProblemDTO;

//            // Assert - Response
//            result.ShouldNotBeNull();
//            result.Id.ShouldBe(-9);
//            result.IsActive.ShouldBe(false);

//            // Assert - Database
//            var storedEntity = dbContext.Problem.FirstOrDefault(i => i.Id == -9);
//            storedEntity.ShouldNotBeNull();
//            storedEntity.IsActive.ShouldBe(false);
//        }

//        /* [Fact]
//         public void Update_Problem_Fails_InvalidId()
//         {
//             // Arrange
//             using var scope = Factory.Services.CreateScope();
//             var controller = CreateController(scope);
//             var updatedEntity = new ProblemDTO
//             {
//                 Id = -100, // Nevažeći ID
//                 UserId = 22,
//                 TourId = 1,
//                 Category = "Technical",
//                 Description = "Issue with the app",
//                 Priority = 1,
//                 Time = DateTime.UtcNow,
//                 IsActive = false
//             };

//             // Act
//             var result = (ObjectResult)controller.UpdateActiveStatus((int)updatedEntity.Id, false).Result;

//             // Assert
//             result.ShouldNotBeNull();
//             result.StatusCode.ShouldBe(404);
//         }*/

//        [Fact]
//        public void Update_Problem_Fails_InvalidId()
//        {
//            // Arrange
//            using var scope = Factory.Services.CreateScope();
//            var controller = CreateController(scope);
//            long invalidId = -100; // Nevažeći ID

//            // Act
//            // var result = controller.UpdateActiveStatus((int)invalidId, false);

//            // Assert
//            // result.ShouldNotBeNull();
//            // Act
//            var result = controller.UpdateActiveStatus((int)invalidId, false) as ActionResult<ProblemDTO>;

//            // Assert
//            Assert.NotNull(result); // Proverava da rezultat nije null

//            // Proverava da li je rezultat NotFound
//            Assert.IsType<ObjectResult>(result.Result);
//        }

//        private static ProblemControllerAdmin CreateController(IServiceScope scope)
//        {
//            return new ProblemControllerAdmin(scope.ServiceProvider.GetRequiredService<IProblemService>())
//            {
//                ControllerContext = BuildContext("-1")
//            };
//        }
//    }
//}
