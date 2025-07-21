namespace Explorer.API.Utilities
{
    public class DatabaseInitializerService : IHostedService
    {

        private readonly IServiceProvider _services;

        public DatabaseInitializerService(IServiceProvider services)
        {
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var initializer = services.GetRequiredService<DatabaseInitializer>();

                try
                {
                    // Perform database initialization here
                    await initializer.StakeholdersDatabaseAsync(services);
                  //  await initializer.ToursDatabaseAsync(services);
                    //await initializer.BlogsDatabaseAsync(services);
                    //await initializer.PaymentsDatabaseAsync(services);
                    //await initializer.EncountersDatabaseAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DatabaseInitializerService>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
