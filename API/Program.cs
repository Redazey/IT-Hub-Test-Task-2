using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=db.sqlite"));

builder.Services.AddScoped<IRollItemService, RollItemService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
