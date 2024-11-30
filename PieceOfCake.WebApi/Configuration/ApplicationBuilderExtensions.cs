using Microsoft.EntityFrameworkCore;
using PieceOfCake.DAL;

namespace PieceOfCake.WebApi.Configuration;

public static class ApplicationBuilderExtensions
{
    public const string CorsPolicyAllowAllOrigins = "CorsPolicyAllowAllOrigins";
    public const string CorsPolicyAllowSpecificOrigin = "CorsPolicyAllowSpecificOrigin";

    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var sqlConnectionString =
            builder.Configuration["SQL_ConnectionStrings"]
            ?? throw new InvalidOperationException("Connection string" + "'SqlDatabase' not found.");

        builder.Services.AddDbContext<PocDbContext>(options =>
            options.UseSqlServer(sqlConnectionString));

        return builder;
    }
    
    public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins");

        if(!string.IsNullOrEmpty(allowedOrigins?.Value))
        {
            var origins = allowedOrigins.Value.Split(";");

            builder.Services.AddCors(options =>

                options.AddPolicy(name: CorsPolicyAllowSpecificOrigin,
                    corsPolicyBuilder =>
                    {
                        corsPolicyBuilder.WithOrigins(origins);
                        corsPolicyBuilder.AllowAnyMethod();
                        corsPolicyBuilder.AllowAnyHeader();
                    }));
        }
        else
        {
            builder.Services.AddCors(options =>

           options.AddPolicy(name: CorsPolicyAllowAllOrigins,
               corsPolicyBuilder =>
               {
                   corsPolicyBuilder.AllowAnyOrigin();
                   corsPolicyBuilder.AllowAnyMethod();
                   corsPolicyBuilder.AllowAnyHeader();
               }));
        }

        return builder;
    }
}
