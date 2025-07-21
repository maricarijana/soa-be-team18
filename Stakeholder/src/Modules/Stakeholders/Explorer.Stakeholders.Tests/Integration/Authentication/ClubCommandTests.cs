//using Explorer.API.Controllers.Administrator.Administration;
//using Explorer.API.Controllers.Tourist;
//using Explorer.BuildingBlocks.Core.UseCases;
//using Explorer.Stakeholders.API.Dtos;
//using Explorer.Stakeholders.API.Public;
//using Explorer.Stakeholders.Infrastructure.Database;
//using Explorer.Tours.API.Dtos;
//using Explorer.Tours.API.Public.Administration;
//using Explorer.Tours.Infrastructure.Database;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ClubTagsEnum = Explorer.Stakeholders.Core.Domain.Club.ClubTags;


//namespace Explorer.Stakeholders.Tests.Integration.Authentication;
//[Collection("Sequential")]
//public class ClubCommandTests:BaseStakeholdersIntegrationTest
//{

//    public ClubCommandTests(StakeholdersTestFactory factory) : base(factory) { }

//    [Fact]
//    public void Creates()
//    {
//        //Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
//        var newEntity = new ClubDto
//        {
//            Name = "Adventure Seekers Club",
//            Description = "A community for thrill-seekers and explorers to share travel stories, plan group adventures, and discover new, off-the-beaten-path destinations around the world.",
//            Image = "newImage",
//            UserId = 12,
//            UserIds = new List<long>(),
//            Tags = new List<ClubDto.ClubTags> { ClubDto.ClubTags.Adventure, ClubDto.ClubTags.Wildlife }

//        };
//        // Act
//        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ClubDto;

//        // Assert - Response
//        result.ShouldNotBeNull();
//        result.Id.ShouldNotBe(0);
//        result.Name.ShouldBe(newEntity.Name);
//        result.Tags.ShouldBe(newEntity.Tags); 

//        // Assert - Database
//        var storedEntity = dbContext.Clubs.FirstOrDefault(i => i.Name == newEntity.Name);
//        storedEntity.ShouldNotBeNull();
//        storedEntity.Id.ShouldBe(result.Id);

//    }

//    [Fact]
//    public void Create_fails_invalid_data()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var updatedEntity = new ClubDto
//        {
//            Description = "Test",
//            Image= "Test",
//            UserId=2,
//            UserIds = new List<long>(),
//            Tags = new List<ClubDto.ClubTags>() // Prazna lista tagova

//        };

//        // Act
//        var result = (ObjectResult)controller.Create(updatedEntity).Result;

//        // Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(400);
//    }

//    [Fact]
//    public void Updates()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
//        var updatedEntity = new ClubDto
//        {
//            Id = -1,
//            Name = "Wanderlust Explorers",
//            Description = "Join a community of passionate travelers eager to share their experiences, find travel companions, and discover hidden gems across the globe.",
//            Image="littleImage",
//            UserId=22,
//            UserIds = new List<long>(),
//            Tags = new List<ClubDto.ClubTags> { ClubDto.ClubTags.Culture, ClubDto.ClubTags.Photography }


//        };

//        // Act
//        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ClubDto;

//        // Assert - Response
//        result.ShouldNotBeNull();
//        result.Id.ShouldBe(-1);
//        result.Name.ShouldBe(updatedEntity.Name);
//        result.Description.ShouldBe(updatedEntity.Description);
//        result.Image.ShouldBe(updatedEntity.Image);
//        result.Tags.ShouldBe(updatedEntity.Tags); // Provera tagova


//        // Assert - Database
//        var storedEntity = dbContext.Clubs.FirstOrDefault(i => i.Name == "Wanderlust Explorers");
//        storedEntity.ShouldNotBeNull();
//        storedEntity.Description.ShouldBe(updatedEntity.Description);
//        var oldEntity = dbContext.Clubs.FirstOrDefault(i => i.Name == "Mountaineering Society of Vojvodina");
//        oldEntity.ShouldBeNull();
//    }
//    [Fact]
//    public void Update_fails_invalid_id()
//    {
//        // Arrange
//        using var scope = Factory.Services.CreateScope();
//        var controller = CreateController(scope);
//        var updatedEntity = new ClubDto
//        {
//            Id = -100,
//            Name = "Test",
//            Description = "TestDescription",
//            Image= "TestImage",
//            UserId= 22,
//            UserIds = new List<long>(),
//            Tags = new List<ClubDto.ClubTags> { ClubDto.ClubTags.Adventure }
//        };

//        // Act
//        var result = (ObjectResult)controller.Update(updatedEntity).Result;

//        // Assert
//        result.ShouldNotBeNull();
//        result.StatusCode.ShouldBe(404);
//    }


//    private static ClubController CreateController(IServiceScope scope)
//    {
//        return new ClubController(scope.ServiceProvider.GetRequiredService<IClubService>(), scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>(), scope.ServiceProvider.GetRequiredService<IImageService>())
//        {
//            ControllerContext = BuildContext("-1")
//        };
//    }

//}
