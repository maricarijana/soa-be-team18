using Explorer.API.Controllers;
using Explorer.API.Startup;
using Explorer.API.Utilities;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});




builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
const string corsPolicy = "_corsPolicy";
builder.Services.ConfigureCors(corsPolicy);
builder.Services.ConfigureAuth();

builder.Services.RegisterModules();



/*Zakomentarisati DatabaseInitializer i DatabaseInitializerService pri pokretanju testova */
/* Druga opcija je pokrenuti samo jednom pred kontrolnu tacku bez komentarisanja, pa posle raditi normalno */

//builder.Services.AddTransient<DatabaseInitializer>();
//builder.Services.AddHostedService<DatabaseInitializerService>(); 

builder.Services.AddGrpc().AddJsonTranscoding();



var app = builder.Build();

//initial migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
    db.Database.Migrate();
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<StakeholdersProtoController>();
Console.WriteLine("Stakeholders gRPC service mapped: StakeholdersProtoController");


var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Using DB connection: {connStr}");

app.Run();

// Required for automated tests
namespace Explorer.API
{
    public partial class Program { }
}