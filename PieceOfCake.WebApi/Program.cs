using Microsoft.EntityFrameworkCore;
using PieceOfCake.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var sqlConnectionString =
    builder.Configuration.GetConnectionString("SqlDatabase")
        ?? throw new InvalidOperationException("Connection string" + "'SqlDatabase' not found.");

builder.Services.AddDbContext<PocDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
