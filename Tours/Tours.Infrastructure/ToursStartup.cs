using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tours.Infrastructure.Database.Repositories;
using Tours.Infrastructure.Database;
using Tours.Core.Mappers;
using Tours.Application.Public.Author;
using Tours.Core.UseCases.Author;
using Tours.Core.Domain;
using Tours.Core.Domain.RepositoryInterfaces;
using AutoMapper;
using Tours.Core.UseCases;
using Tours.Core.Domain.Execution;
using Tours.Application.Public;


namespace Tours.Infrastructure;

public static class ToursStartup
{
    public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
    {
        // Registers all profiles since it works on the assembly
        object value = services.AddAutoMapper(typeof(ToursProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }

    private static void SetupCore(IServiceCollection services)
    {

        services.AddScoped<IKeyPointService, KeyPointService>();
        services.AddScoped<ITourService, TourService>();
        //services.AddScoped<IObjectService, ObjectService>();

        services.AddScoped<ITourOverviewService, TourOverviewService>();
        services.AddScoped<ITourReviewService, TourReviewService>();
        services.AddScoped<IPositionSimulatorService, PositionSimulationService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<IPurchaseService, PurchaseService>();

        services.AddScoped<IImageService,ImageService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
       // services.AddScoped(typeof(ICrudRepository<Equipment>), typeof(CrudDatabaseRepository<Equipment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<KeyPoint>), typeof(CrudDatabaseRepository<KeyPoint, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourReview>), typeof(CrudDatabaseRepository<TourReview, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<Tour>), typeof(CrudDatabaseRepository<Tour, ToursContext>));
        //services.AddScoped(typeof(ICrudRepository<Core.Domain.Object>), typeof(CrudDatabaseRepository<Core.Domain.Object, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<PositionSimulator>), typeof(CrudDatabaseRepository<PositionSimulator, ToursContext>));
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        services.AddScoped<ITourPurchaseRepository, PurchaseRepository>();


        services.AddScoped<ITourRepository, TourRepository>();
        services.AddScoped<IKeyPointRepository, KeyPointRepository>();
        services.AddScoped<ITourReviewRepository, TourReviewRepository>();
        services.AddScoped<IPositionSimulatorRepository, PositionSimulatorRepository>();


        services.AddDbContext<ToursContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("tours"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));

    }
}