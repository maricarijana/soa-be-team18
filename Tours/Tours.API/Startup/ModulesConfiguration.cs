//using Explorer.Blog.Infrastructure;

//using Explorer.Blog.Infrastructure;
using Tours.Infrastructure;
//using Explorer.Tours.Infrastructure;
//using Explorer.Payments.Infrastructure;
//using Explorer.Encounter.Infrastructure;

namespace Tours.API.Startup;

public static class ModulesConfiguration
{
    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        services.ConfigureToursModule();
        //services.ConfigureToursModule();
        //services.ConfigureBlogModule();
        //services.ConfigurePaymentsModule();
        //services.ConfigureEncounterModule();

        return services;
    }
}