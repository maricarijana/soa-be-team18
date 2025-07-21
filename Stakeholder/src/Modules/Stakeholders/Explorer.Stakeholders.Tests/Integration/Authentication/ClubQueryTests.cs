using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Microsoft.AspNetCore.Hosting;

namespace Explorer.Stakeholders.Tests.Integration.Authentication;

[Collection("Sequential")]
public class ClubQueryTests : BaseStakeholdersIntegrationTest
{
    public ClubQueryTests(StakeholdersTestFactory factory) : base(factory) { }
    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<ClubDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(2);
        result.TotalCount.ShouldBe(2);

    }

    private static ClubController CreateController(IServiceScope scope)
    {
        return new ClubController(scope.ServiceProvider.GetRequiredService<IClubService>(), scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>(), scope.ServiceProvider.GetRequiredService<IImageService>())
        {
            ControllerContext = BuildContext("-1")
        };


    }
}

