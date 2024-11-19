using Microsoft.Extensions.DependencyInjection;
using Moq;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Tests.Common.Fakes;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Tests;
public class ServicesRegistration
{
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IProductRepository> _productRepoMock;
    private Mock<IMeasureUnitRepository> _measureUnitRepoMock;
    private Mock<IMealOfTheDayTypeRepository> _mealOfTheDayTypeRepository;
    
    public ServicesRegistration ()
    {
        _measureUnitRepoMock = new Mock<IMeasureUnitRepository>();
        _measureUnitRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .ReturnsAsync(null as MeasureUnit);

        _productRepoMock = new Mock<IProductRepository>();
        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(null as Product);

        _mealOfTheDayTypeRepository = new Mock<IMealOfTheDayTypeRepository>();
        _mealOfTheDayTypeRepository
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .ReturnsAsync(null as MealOfTheDayType);

        _uowMock = new Mock<IUnitOfWork>();
        _uowMock.Setup(x => x.MeasureUnitRepository).Returns(_measureUnitRepoMock.Object);
        _uowMock.Setup(x => x.ProductRepository).Returns(_productRepoMock.Object);
        _uowMock.Setup(x => x.MealOfTheDayTypeRepository).Returns(_mealOfTheDayTypeRepository.Object);
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
        services.AddSingleton(_uowMock.Object);

        return services.BuildServiceProvider();
    }
}
