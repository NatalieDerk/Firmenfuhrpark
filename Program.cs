using Microsoft.EntityFrameworkCore;
using Backend.Db_tables;
using Backend.Hubs;
using Microsoft.AspNetCore.SignalR;



var builder = WebApplication.CreateBuilder(args);

// Verbindung zur Datenbank hinzufügen
builder.Services.AddDbContext<ApplicationDBContext>
(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options => 
    { options.AddPolicy("AllowAngularDev", policy => 
        { policy.WithOrigins("http://localhost:4200") 
        .AllowAnyHeader() 
        .AllowAnyMethod() 
        .AllowCredentials(); 
        }); 
    }); 

// Controller hinzufügen
builder.Services.AddControllers() 
    .AddJsonOptions(options => 
    { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

// Swagger-Dinste hinzufügen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddDirectoryBrowser();

var app = builder.Build();

app.UseCors("AllowAngularDev"); 
 

// Swagger-Middleware aktiviren
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Generiert die Swagger-Dokumentation
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fuhpark API");
        c.RoutePrefix = "swagger"; // Swagger-UI über Root-URL erreichbar (http://localhost:5156)
    });
}


app.MapControllers(); 
app.MapHub<BookingHub>("/bookingHub"); 
app.Run();