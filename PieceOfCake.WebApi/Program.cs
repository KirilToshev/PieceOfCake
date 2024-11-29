using Microsoft.EntityFrameworkCore;
using PieceOfCake.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();
builder.Services.AddServiceRegistration();
builder.Services.AddAutoMapper();

var sqlConnectionString =
    builder.Configuration.GetConnectionString("SqlDatabase")
        ?? throw new InvalidOperationException("Connection string" + "'SqlDatabase' not found.");
builder.Services.AddDatabase(sqlConnectionString);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Piece Of Cake Api");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
