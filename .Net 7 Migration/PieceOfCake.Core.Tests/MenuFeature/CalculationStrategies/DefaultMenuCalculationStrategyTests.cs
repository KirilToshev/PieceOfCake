using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Tests.Common;
using PieceOfCake.Tests.Common.Fakes;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Tests.MenuFeature.CalculationStrategies;

public class DefaultMenuCalculationStrategyTests
{
    private IResources _resources;
    private Fixture _fixture;
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IProductRepository> _productRepoMock;
    private Mock<IMeasureUnitRepository> _measureUnitRepoMock;
    private Mock<IMealOfTheDayTypeRepository> _mealOfTheDayTypeRepository;
    private IDishRepository _dishRepoMock;
    private DishFakes _dishFakes;

    [SetUp]
    public void BeforeEachTest ()
    {
        _fixture = new Fixture();
        IServiceCollection services = new ServiceCollection();
        services.AddResources();
        var serviceProvider = services.BuildServiceProvider();
        _resources = serviceProvider.GetService<IResources>();

        _measureUnitRepoMock = new Mock<IMeasureUnitRepository>();
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns<MeasureUnit>(null);

        _productRepoMock = new Mock<IProductRepository>();
        _productRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns<Product>(null);

        _mealOfTheDayTypeRepository = new Mock<IMealOfTheDayTypeRepository>();
        _mealOfTheDayTypeRepository
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .Returns<MealOfTheDayType>(null);

        _uowMock = new Mock<IUnitOfWork>();
        _uowMock.Setup(x => x.MeasureUnitRepository).Returns(_measureUnitRepoMock.Object);
        _uowMock.Setup(x => x.ProductRepository).Returns(_productRepoMock.Object);
        _uowMock.Setup(x => x.MealOfTheDayTypeRepository).Returns(_mealOfTheDayTypeRepository.Object);

        _dishFakes = new DishFakes(_resources, _uowMock.Object);
    }

    [Test]
    public void Calculate_Should_Fill_Calendar_For_Two_Days_With_Breakfast_Lunch_And_Dinner_For_Two_People ()
    {
        var breakfastDish = _dishFakes.Breakfast(2);

        throw new NotImplementedException();
        Assert.That(TestsConstants.Dishes.BREAKFAST_DISH == breakfastDish.Name.Value);
    }
}
