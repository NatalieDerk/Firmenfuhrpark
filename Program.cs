using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDBContext>
(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "dist";
});

var app = builder.Build();

app.UseStaticFiles();
app.Use.SpaStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();