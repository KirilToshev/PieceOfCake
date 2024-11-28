using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.DAL;
using PieceOfCake.DAL.Repositories;

namespace PieceOfCake.WebApi;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
    {
        services.AddSingleton<IResources, Resources>();

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PocDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
