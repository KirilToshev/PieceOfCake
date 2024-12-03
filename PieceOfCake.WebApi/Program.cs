using Microsoft.AspNetCore.Localization;
using PieceOfCake.Core.Common;
using PieceOfCake.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(Common.SupportedLanguages[0]);
    options.AddSupportedCultures(Common.SupportedLanguages);
    options.AddSupportedUICultures(Common.SupportedLanguages);
});
builder.Services.AddServiceRegistration();

builder.Services.AddAutoMapper();

builder.ConfigureDatabase();
builder.ConfigureCors();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseRequestLocalization();
// Configure the HTTP request pipeline.
app.UseSwaggerUI();

app.UseCors(PieceOfCake.WebApi.Configuration.ApplicationBuilderExtensions.CorsPolicyAllowAllOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ConfigureExceptionHandler();

app.Run();

// TODO: Add Authentication/Authorization
// TODO: Add Logger
// TODO: Seed database
// TODO: Add Cache
