using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using PieceOfCake.Core.Common;
using PieceOfCake.DAL;
using PieceOfCake.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(Common.SupportedLanguages[0]);
    options.AddSupportedCultures(Common.SupportedLanguages);
    options.AddSupportedUICultures(Common.SupportedLanguages);
});
builder.Services.AddServiceRegistration();

builder.Services.AddAutoMapper(typeof(Program));

builder.ConfigureDatabase();
builder.ConfigureCors();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        ValidateIssuer = true,
        ValidateAudience = true
    };
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseRequestLocalization();
// Configure the HTTP request pipeline.
DbManagementService.MigrationInitialization(app);
app.UseSwaggerUI();

app.UseCors(PieceOfCake.WebApi.Configuration.ApplicationBuilderExtensions.CorsPolicyAllowAllOrigins);

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.ConfigureExceptionHandler();

app.Run();

// TODO: Add Authentication/Authorization
// TODO: Add Logger
// TODO: Seed database
// TODO: Add Cache
// TODO: Options pattern
