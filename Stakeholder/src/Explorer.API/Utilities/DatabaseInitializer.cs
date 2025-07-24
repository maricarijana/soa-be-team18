
using Explorer.Stakeholders.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Explorer.API.Utilities;

public class DatabaseInitializer
{

    private readonly IConfiguration _configuration;

    public DatabaseInitializer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task StakeholdersDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var scriptPath = Path.Combine(
         AppContext.BaseDirectory, "..", "..", "..", "Scripts", "Stakeholders", "stakeholders-data.sql");

        // Normalize the path for different environments
        scriptPath = Path.GetFullPath(scriptPath);

        if (!File.Exists(scriptPath))
        {
            throw new FileNotFoundException($"SQL script not found at path: {scriptPath}");
        }
        var sql = await File.ReadAllTextAsync(scriptPath);

        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }

    //public async Task EncountersDatabaseAsync(IServiceProvider serviceProvider)
    //{
    //    using var scope = serviceProvider.CreateScope();
    //    var dbContext = scope.ServiceProvider.GetRequiredService<EncounterContext>();

    //    var scriptPath = Path.Combine(
    //     AppContext.BaseDirectory, "..", "..", "..", "Scripts", "Encounters", "encounters-data.sql");

    //    // Normalize the path for different environments
    //    scriptPath = Path.GetFullPath(scriptPath);

    //    if (!File.Exists(scriptPath))
    //    {
    //        throw new FileNotFoundException($"SQL script not found at path: {scriptPath}");
    //    }
    //    var sql = await File.ReadAllTextAsync(scriptPath);

    //    await dbContext.Database.ExecuteSqlRawAsync(sql);
    //}

    //public async Task BlogsDatabaseAsync(IServiceProvider serviceProvider)
    //{
    //    using var scope = serviceProvider.CreateScope();
    //    var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

    //    var scriptPath = Path.Combine(
    //     AppContext.BaseDirectory, "..", "..", "..", "Scripts", "Blogs", "blogs-data.sql");

    //    // Normalize the path for different environments
    //    scriptPath = Path.GetFullPath(scriptPath);

    //    if (!File.Exists(scriptPath))
    //    {
    //        throw new FileNotFoundException($"SQL script not found at path: {scriptPath}");
    //    }
    //    var sql = await File.ReadAllTextAsync(scriptPath);

    //    await dbContext.Database.ExecuteSqlRawAsync(sql);
    //}

    //public async Task ToursDatabaseAsync(IServiceProvider serviceProvider)
    //{
    //    using var scope = serviceProvider.CreateScope();
    //    var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

    //    var scriptPath = Path.Combine(
    //     AppContext.BaseDirectory, "..", "..", "..", "Scripts", "Tours", "tours-data.sql");

    //    // Normalize the path for different environments
    //    scriptPath = Path.GetFullPath(scriptPath);

    //    if (!File.Exists(scriptPath))
    //    {
    //        throw new FileNotFoundException($"SQL script not found at path: {scriptPath}");
    //    }
    //    var sql = await File.ReadAllTextAsync(scriptPath);

    //    await dbContext.Database.ExecuteSqlRawAsync(sql);
    //}


    //public async Task PaymentsDatabaseAsync(IServiceProvider serviceProvider)
    //{
    //    using var scope = serviceProvider.CreateScope();
    //    var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

    //    var scriptPath = Path.Combine(
    //     AppContext.BaseDirectory, "..", "..", "..", "Scripts", "Payments", "payments-data.sql");

    //    // Normalize the path for different environments
    //    scriptPath = Path.GetFullPath(scriptPath);

    //    if (!File.Exists(scriptPath))
    //    {
    //        throw new FileNotFoundException($"SQL script not found at path: {scriptPath}");
    //    }
    //    var sql = await File.ReadAllTextAsync(scriptPath);

    //    await dbContext.Database.ExecuteSqlRawAsync(sql);
    //}


}
