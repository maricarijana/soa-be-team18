
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos; 
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Database;
using FluentResults;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Xml.Linq;

namespace Explorer.Stakeholders.Tests.Integration.PersonTests;

[Collection("Sequential")]
public class PersonTests : BaseStakeholdersIntegrationTest
{
    public PersonTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new PersonDto
        {
            Id = -11,
            UserId = -11,
            Name = "string",
            Surname = "string",
            Email = "string@gmail.com",
            ImageUrl = "string",
            Biography = "string",
            Motto = "string"
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as PersonDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(updatedEntity.Id);
        result.UserId.ShouldBe(updatedEntity.UserId);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Surname.ShouldBe(updatedEntity.Surname);
        result.Email.ShouldBe(updatedEntity.Email);
        result.ImageUrl.ShouldBe(updatedEntity.ImageUrl);
        result.Biography.ShouldBe(updatedEntity.Biography);
        result.Motto.ShouldBe(updatedEntity.Motto);

        // Assert - Database
        var storedEntity = dbContext.People.FirstOrDefault(i => i.Id == updatedEntity.Id);
        storedEntity.Email.ShouldBe(updatedEntity.Email);
        storedEntity.ShouldNotBeNull();
        storedEntity.Name.ShouldBe(updatedEntity.Name);
        storedEntity.Surname.ShouldBe(updatedEntity.Surname);
        storedEntity.Biography.ShouldBe(updatedEntity.Biography);

        var oldEntity = dbContext.People.FirstOrDefault(i => i.Email == "autor1@gmail.com");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void GetPersonById()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var actionResult = controller.Get(-11);
        var objectResult = actionResult as ObjectResult;           
        var result = objectResult?.Value as PersonDto; 


        // Assert
        result.ShouldNotBeNull();
        result.UserId.ShouldBe(-11);
        result.Name.ShouldBe("string");
        result.Surname.ShouldBe("string");
        result.Email.ShouldBe("string@gmail.com");
        result.ImageUrl.ShouldBe("string");
        result.Biography.ShouldBe("string");
        result.Motto.ShouldBe("string");
    }


    private static Explorer.API.Controllers.Tourist.PersonController CreateController(IServiceScope scope)
    {
        return new Explorer.API.Controllers.Tourist.PersonController(scope.ServiceProvider.GetRequiredService<IPersonService>(), scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>(), scope.ServiceProvider.GetRequiredService<IImageService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
