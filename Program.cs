using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;


var builder = WebApplication.CreateBuilder(args);

// Verbindung zur Datenbank hinzufügen
builder.Services.AddDbContext<ApplicationDBContext>
(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options => 
    { options.AddDefaultPolicy("AllowAngularDev", policy => 
        { policy.WithOrigins("http://localhost:4200") 
        .AllowAnyHeader() 
        .AllowAnyMethod() 
        .AllowCredentials(); 
        }); 
    }); 

// Controller hinzufügen
builder.Services.AddControllers() .AddJsonOptions(options => 
    { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

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

builder.Services.AddControllers()
.AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "dist";
});

var app = builder.Build();

app.UseCors("AllowAngularDev"); 
app.UseStaticFiles(); 
app.Use.SpaStaticFiles(); 
app.MapControllers(); 
app.MapFallbackToFile("index.html"); 
app.MapHub<BookingHub>; 
app.Run();