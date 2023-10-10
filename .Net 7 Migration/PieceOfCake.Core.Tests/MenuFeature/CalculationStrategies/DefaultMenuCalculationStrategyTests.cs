using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Core.MenuFeature.CalculationStrategies;
using PieceOfCake.Core.MenuFeature.Calendar;
using PieceOfCake.Tests.Common;
using PieceOfCake.Tests.Common.Fakes;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Tests.MenuFeature.CalculationStrategies;

public class DefaultMenuCalculationStrategyTests
{
    private IResources _resources;
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IProductRepository> _productRepoMock;
    private Mock<IMeasureUnitRepository> _measureUnitRepoMock;
    private Mock<IMealOfTheDayTypeRepository> _mealOfTheDayTypeRepository;
    private IDishRepository _dishRepoMock;
    private IDishFakes _dishFakes;
    private ITimePeriodFakes _timePeriodFakes;
    private IMealOfTheDayTypeFakes _mealOfTheDayTypeFakes;

    public DefaultMenuCalculationStrategyTests ()
    {
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

        var diProvider = new DIProvider(_uowMock.Object);
        _dishFakes = diProvider.GetRequiredService<IDishFakes> ();
        _timePeriodFakes = diProvider.GetRequiredService<ITimePeriodFakes> ();
        _mealOfTheDayTypeFakes = diProvider.GetRequiredService<IMealOfTheDayTypeFakes> ();
    }

    [SetUp]
    public void BeforeEachTest ()
    {
        
    }

    [Test]
    public void Calculate_Should_Fill_Calendar_For_One_Day_With_Breakfast_For_One_Person ()
    {
        var breakfastDish = _dishFakes.Breakfast(2);
        var lunchDish = _dishFakes.Lunch(2);
        var dishes = new[] { breakfastDish, lunchDish };
        ushort numberOfPeople = 1;

        var menuCalendar = new MenuCalendar(
            _timePeriodFakes.OneDay,
            numberOfPeople,
            new[]
            {
                _mealOfTheDayTypeFakes.Breakfast
            });

        var sut = new DefaultMenuCalculationStrategy(_resources);
        var result = sut.Calculate(menuCalendar, dishes);

        Assert.That(result.Value.Count() == 1);
        var calendarItem = result.Value.FirstOrDefault()!;
        Assert.That(calendarItem.Date == _timePeriodFakes.OneDay.StartDate
            && calendarItem.Date == _timePeriodFakes.OneDay.EndDate);
        Assert.That(calendarItem.MealOfTheDayTypes.Count() == 1);
        var calendarMealOfTheDayType = calendarItem.MealOfTheDayTypes.FirstOrDefault()!;
        Assert.That(calendarMealOfTheDayType.Id == _mealOfTheDayTypeFakes.Breakfast.Id);
        Assert.That(calendarMealOfTheDayType.Dishes.Count() == numberOfPeople);
        var dish = calendarMealOfTheDayType.Dishes.FirstOrDefault()!;
        Assert.That(dish.Id == breakfastDish.Id);
    }

    [Test]
    public void Calculate_Should_Fill_Calendar_For_Two_Days_With_Breakfast_Lunch_And_Dinner_For_Two_People ()
    {
        var breakfastDish = _dishFakes.Breakfast(2);
        var lunchDish = _dishFakes.Lunch(2);
        var dinnerDish = _dishFakes.Dinner(2);
        var dishes = new[] { breakfastDish, lunchDish, dinnerDish };
        ushort numberOfPeople = 2;

        var menuCalendar = new MenuCalendar(
            _timePeriodFakes.TwoDays,
            numberOfPeople,
            new[]
            {
                _mealOfTheDayTypeFakes.Breakfast,
                _mealOfTheDayTypeFakes.Lunch,
                _mealOfTheDayTypeFakes.Dinner
            });

        var sut = new DefaultMenuCalculationStrategy(_resources);
        var result = sut.Calculate(menuCalendar, dishes);

        Assert.That(result.Value.Count() == 2);
        foreach ( var calendarItem in result.Value )
        {
            Assert.That(_timePeriodFakes.TwoDays.IsInPeriod(calendarItem.Date));
            Assert.That(calendarItem.MealOfTheDayTypes.Count() == 3);
            foreach (var calendarMealOfTheDayType in calendarItem.MealOfTheDayTypes)
            {
                Assert.IsTrue(menuCalendar.MealOfTheDayTypes.ContainsKey(calendarMealOfTheDayType.Id));
                Assert.That(calendarMealOfTheDayType.Dishes.Count() == 2);
                foreach (var dish in calendarMealOfTheDayType.Dishes)
                {
                    Assert.IsTrue(dishes.Select(x => x.Id).Contains(dish.Id));
                }
            }
        }
    }

    [Test]
    public void Calculate_Should_Fill_Calendar_For_Two_Days_With_Lunch_And_Dinner_For_Two_People ()
    {
        var lunchAndDinnerDish3Servings = _dishFakes.LunchAndDinner(TestsConstants.Dishes.LUNCH_AND_DINNER_DISH + "with 3 servings", 3);
        var lunchAndDinnerDish2Servings = _dishFakes.LunchAndDinner(TestsConstants.Dishes.LUNCH_AND_DINNER_DISH + "with 2 servings", 2);
        var dinnerDish = _dishFakes.Dinner(1);
        var dishes = new[] { lunchAndDinnerDish3Servings, lunchAndDinnerDish2Servings, dinnerDish };
        ushort numberOfPeople = 2;

        var menuCalendar = new MenuCalendar(
            _timePeriodFakes.ThreeDays,
            numberOfPeople,
            new[]
            {
                _mealOfTheDayTypeFakes.Lunch,
                _mealOfTheDayTypeFakes.Dinner
            });

        var sut = new DefaultMenuCalculationStrategy(_resources);
        var result = sut.Calculate(menuCalendar, dishes);
        int dishesServingsIndex = 0;
        var expectedServings = new[]
        {
            lunchAndDinnerDish3Servings.Id,
            lunchAndDinnerDish3Servings.Id,
            lunchAndDinnerDish3Servings.Id,
            lunchAndDinnerDish2Servings.Id,
            lunchAndDinnerDish2Servings.Id,
            lunchAndDinnerDish3Servings.Id,
            lunchAndDinnerDish3Servings.Id,
            lunchAndDinnerDish3Servings.Id,
            lunchAndDinnerDish2Servings.Id,
            lunchAndDinnerDish2Servings.Id,
            dinnerDish.Id,
            lunchAndDinnerDish3Servings.Id,
            lunchAndDinnerDish3Servings.Id,
        };

        Assert.That(result.Value.Count() == 3);
        foreach (var calendarItem in result.Value)
        {
            Assert.That(_timePeriodFakes.ThreeDays.IsInPeriod(calendarItem.Date));
            Assert.That(calendarItem.MealOfTheDayTypes.Count() == 2);
            foreach (var calendarMealOfTheDayType in calendarItem.MealOfTheDayTypes)
            {
                Assert.IsTrue(menuCalendar.MealOfTheDayTypes.ContainsKey(calendarMealOfTheDayType.Id));
                Assert.That(calendarMealOfTheDayType.Dishes.Count() == 2);
                foreach (var dish in calendarMealOfTheDayType.Dishes)
                {
                    Assert.IsTrue(dishes.Select(x => x.Id).Contains(dish.Id));
                    Assert.IsTrue(expectedServings[dishesServingsIndex] == dish.Id);
                    dishesServingsIndex++;
                }
            }
        }
    }

    [Test]
    public void Calculate_Should_Fill_Calendar_For_One_Day_With_Lunch_And_Dinner_For_Two_People ()
    {
        var lunchAndDinnerDish1 = _dishFakes.LunchAndDinner(TestsConstants.Dishes.LUNCH_AND_DINNER_DISH + "1", 1);
        var lunchAndDinnerDish2 = _dishFakes.LunchAndDinner(TestsConstants.Dishes.LUNCH_AND_DINNER_DISH + "2", 1);
        var dinnerDish = _dishFakes.Dinner(1);
        var dishes = new[] { lunchAndDinnerDish1, lunchAndDinnerDish2, dinnerDish };
        ushort numberOfPeople = 2;

        var menuCalendar = new MenuCalendar(
            _timePeriodFakes.OneDay,
            numberOfPeople,
            new[]
            {
                _mealOfTheDayTypeFakes.Lunch,
                _mealOfTheDayTypeFakes.Dinner
            });

        var sut = new DefaultMenuCalculationStrategy(_resources);
        var result = sut.Calculate(menuCalendar, dishes);
        int dishesServingsIndex = 0;
        var expectedServings = new[]
        {
            lunchAndDinnerDish1.Id,
            lunchAndDinnerDish2.Id,
            dinnerDish.Id,
            lunchAndDinnerDish1.Id
        };

        Assert.That(result.Value.Count() == 1);
        foreach (var calendarItem in result.Value)
        {
            Assert.That(_timePeriodFakes.ThreeDays.IsInPeriod(calendarItem.Date));
            Assert.That(calendarItem.MealOfTheDayTypes.Count() == 2);
            foreach (var calendarMealOfTheDayType in calendarItem.MealOfTheDayTypes)
            {
                Assert.IsTrue(menuCalendar.MealOfTheDayTypes.ContainsKey(calendarMealOfTheDayType.Id));
                Assert.That(calendarMealOfTheDayType.Dishes.Count() == 2);
                foreach (var dish in calendarMealOfTheDayType.Dishes)
                {
                    Assert.IsTrue(dishes.Select(x => x.Id).Contains(dish.Id));
                    Assert.IsTrue(expectedServings[dishesServingsIndex] == dish.Id);
                    dishesServingsIndex++;
                }
            }
        }
    }
}
