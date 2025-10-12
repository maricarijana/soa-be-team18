namespace Tours.Infrastructure.Database;
                                                                                                      
public static class DbConnectionStringBuilder
{
    public static string Build(string schemaName)
    {
        //var server = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
        var server = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "database";
        //var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5433";
        var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
        //var database = Environment.GetEnvironmentVariable("DATABASE_SCHEMA") ?? "soa-tours";
        var database = Environment.GetEnvironmentVariable("DATABASE_SCHEMA") ?? "tours-service";
        var schema = Environment.GetEnvironmentVariable("DATABASE_SCHEMA_NAME") ?? schemaName;
        var user = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "postgres";
        var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "super";
        var integratedSecurity = Environment.GetEnvironmentVariable("DATABASE_INTEGRATED_SECURITY") ?? "false";
        var pooling = Environment.GetEnvironmentVariable("DATABASE_POOLING") ?? "true";

        return
            $"Server={server};Port={port};Database={database};SearchPath={schema};User ID={user};Password={password};Integrated Security={integratedSecurity};Pooling={pooling};";
    }
}