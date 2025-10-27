using Microsoft.EntityFrameworkCore;
using Backend.Db_table;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDBContext>
(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();