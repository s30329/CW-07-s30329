using APBD07;
using APBD07.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseInMemoryDatabase("TripsDb"));

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();