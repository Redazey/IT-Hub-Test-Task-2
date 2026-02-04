using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "db.sqlite";

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbName}"));

builder.Services.AddScoped<IRollItemService, RollItemService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
