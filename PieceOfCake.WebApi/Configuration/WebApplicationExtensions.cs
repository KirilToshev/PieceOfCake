using Microsoft.AspNetCore.Diagnostics;

namespace PieceOfCake.WebApi.Configuration;

public static class WebApplicationExtensions
{
    public static WebApplication UseSwaggerUI(this WebApplication app)
    {
        if(app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Piece Of Cake Api");
                options.RoutePrefix = string.Empty;
            });
        }

        return app;
    }

    public static WebApplication ConfigureExceptionHandler(this WebApplication app)
    {
        if(app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(c =>
            c.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if(exception is not null)
                {
                    // TODO: Log exception.
                }

                await Results.Problem().ExecuteAsync(context);
            }));
        }

        return app;
    }
}
