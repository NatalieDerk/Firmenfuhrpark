using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;


var builder = WebApplication.CreateBuilder(args);

// Verbindung zur Datenbank hinzufügen
builder.Services.AddDbContext<ApplicationDBContext>
(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

<<<<<<< HEAD
// Controller hinzufügen
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Swagger-Dinste hinzufügen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger-Middleware aktiviren
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Generiert die Swagger-Dokumentation
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fuhpark API");
        c.RoutePrefix = string.Empty; // Swagger-UI über Root-URL erreichbar (http://localhost:5156)
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();
}
=======
builder.Services.AddControllers();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "dist";
});

var app = builder.Build();

app.UseStaticFiles();
app.Use.SpaStaticFiles();
>>>>>>> 1029d18d8e03322f5bdb506253564d3c1c2bb079
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();