using Tours.API.Controllers;
using Tours.API.Startup;
using Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(90, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });

});
// Add services to the container.
//builder.Services.AddDbContext<ToursContext>(options =>
//    options.UseNpgsql(DbConnectionStringBuilder.Build("tours")));

builder.WebHost.UseWebRoot("wwwroot");

builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
const string corsPolicy = "_corsPolicy";
builder.Services.ConfigureCors(corsPolicy);
builder.Services.ConfigureAuth();

builder.Services.RegisterModules();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddGrpc().AddJsonTranscoding();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ToursContext>();
    db.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<ToursProtoController>();
Console.WriteLine("Stakeholders gRPC service mapped: StakeholdersProtoController");
app.MapGrpcService<KeyPointProtoController>();
Console.WriteLine("KeyPoint gRPC service mapped: KeyPointProtoController");
app.MapGrpcService<TourReviewProtoController>();
Console.WriteLine("TourReview gRPC service mapped: TourReviewProtoController");
app.MapGrpcService<PositionSimulatorProtoController>();
Console.WriteLine("TourReview gRPC service mapped: PositionSimulatorProtoController");


var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Using DB connection: {connStr}");


app.Run();
