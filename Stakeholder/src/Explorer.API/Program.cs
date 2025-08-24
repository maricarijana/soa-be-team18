using Explorer.API.Startup;
using Explorer.API.Utilities;

var builder = WebApplication.CreateBuilder(args);


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

 


var app = builder.Build();



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
app.UseAuthorization();

app.MapControllers();

app.Run();

// Required for automated tests
namespace Explorer.API
{
    public partial class Program { }
}