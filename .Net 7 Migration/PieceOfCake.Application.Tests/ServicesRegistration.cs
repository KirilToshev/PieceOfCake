using Microsoft.Extensions.DependencyInjection;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using PieceOfCake.Tests.Common.Fakes;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;
using NSubstitute;
using System.Linq.Expressions;

namespace PieceOfCake.Application.Tests;
public class ServicesRegistration
{
    private IUnitOfWork _uowMock;
    private IProductRepository _productRepoMock;
    private IMeasureUnitRepository _measureUnitRepoMock;
    private IMealOfTheDayTypeRepository _mealOfTheDayTypeRepository;

    public ServicesRegistration ()
    {
        _measureUnitRepoMock = Substitute.For<IMeasureUnitRepository>();
        _measureUnitRepoMock.FirstOrDefaultAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<MeasureUnit, bool>>>())
            .Returns(Task.FromResult(null as MeasureUnit));

        _productRepoMock = Substitute.For<IProductRepository>();
        _productRepoMock.FirstOrDefaultAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Product, bool>>>())
            .Returns(Task.FromResult(null as Product));

        _mealOfTheDayTypeRepository = Substitute.For<IMealOfTheDayTypeRepository>();
        _mealOfTheDayTypeRepository.FirstOrDefaultAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<MealOfTheDayType, bool>>>())
            .Returns(Task.FromResult(null as MealOfTheDayType));

        _uowMock = Substitute.For<IUnitOfWork>();
        _uowMock.MeasureUnitRepository.Returns(_measureUnitRepoMock);
        _uowMock.ProductRepository.Returns(_productRepoMock);
        _uowMock.MealOfTheDayTypeRepository.Returns(_mealOfTheDayTypeRepository);
    }

    public IServiceProvider Register ()
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
        services.AddSingleton(_uowMock);

        return services.BuildServiceProvider();
    }
}
