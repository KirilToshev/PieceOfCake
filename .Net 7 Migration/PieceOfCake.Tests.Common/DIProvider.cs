using Microsoft.Extensions.DependencyInjection;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Tests.Common.Fakes;
using PieceOfCake.Tests.Common.Fakes.Interfaces;

namespace PieceOfCake.Tests.Common;
public class DIProvider
{
    private IServiceProvider ServiceProvider { get; }

    public DIProvider (IUnitOfWork uowMock)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddLocalization();

        services.AddTransient<IResources, Resources>();
        services.AddSingleton<IDishFakes, DishFakes>();
        services.AddSingleton<IIngredientFakes, IngredientFakes>();
        services.AddSingleton<IMealOfTheDayTypeFakes, MealOfTheDayTypeFakes>();
        services.AddSingleton<IMeasureUnitFakes, MeasureUnitFakes>();
        services.AddSingleton<IProductFakes, ProductFakes>();
        services.AddSingleton<ITimePeriodFakes, TimePeriodFakes>();
        services.AddSingleton(uowMock);
        ServiceProvider = services.BuildServiceProvider();
    }

    public T GetRequiredService<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();
}
