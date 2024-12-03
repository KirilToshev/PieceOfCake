using PieceOfCake.Application.DishFeature.Services;
using PieceOfCake.Application.IngredientFeature.Services;
using PieceOfCake.Application.MenuFeature.Services;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.DAL.Repositories;
namespace PieceOfCake.WebApi.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
    {
        services.AddSingleton<IResources, Resources>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        #region Repositories registration
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IMeasureUnitRepository, MeasureUnitRepository>();
        services.AddScoped<IMealOfTheDayTypeRepository, MealOfTheDayTypeRepository>();
        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        #endregion

        #region Services registration
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IMeasureUnitService, MeasureUnitService>();
        services.AddScoped<IMealOfTheDayTypeService, MealOfTheDayTypeService>();
        services.AddScoped<IDishService, DishService>();
        services.AddScoped<IMenuService, MenuService>();
        #endregion

        return services;
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {

        services.AddAutoMapper(typeof(Program));
        return services;
    }
}
