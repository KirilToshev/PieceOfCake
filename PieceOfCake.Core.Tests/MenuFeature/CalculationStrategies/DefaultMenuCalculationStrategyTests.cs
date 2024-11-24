using NUnit.Framework;
using PieceOfCake.Core.MenuFeature.CalculationStrategies;
using PieceOfCake.Core.MenuFeature.Calendar;
using PieceOfCake.Tests.Common;
using PieceOfCake.Tests.Common.Fakes.Interfaces;

namespace PieceOfCake.Core.Tests.MenuFeature.CalculationStrategies;

public class DefaultMenuCalculationStrategyTests : TestsBase
{
    private IDishFakes _dishFakes;
    private ITimePeriodFakes _timePeriodFakes;
    private IMealOfTheDayTypeFakes _mealOfTheDayTypeFakes;

    public DefaultMenuCalculationStrategyTests() 
    {
        
        _dishFakes = GetRequiredService<IDishFakes>();
        _timePeriodFakes = GetRequiredService<ITimePeriodFakes> ();
        _mealOfTheDayTypeFakes = GetRequiredService<IMealOfTheDayTypeFakes> ();
    }

    [SetUp]
    public void BeforeEachTest ()
    {
        
    }

    [Test]
    public void Calculate_Should_Error_Message_When_There_Are_No_Dishes_Of_MealType ()
    {
        var breakfastDish = _dishFakes.Breakfast();
        var dishes = new[] { breakfastDish };
        ushort numberOfPeople = 1;

        var menuCalendar = new MenuCalendar(
            _timePeriodFakes.OneDay,
            numberOfPeople,
            new[]
            {
                _mealOfTheDayTypeFakes.Lunch,
                _mealOfTheDayTypeFakes.Dinner
            });

        var sut = new DefaultMenuCalculationStrategy(Resources);
        var result = sut.Calculate(menuCalendar, dishes);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"There are not enough dishes of menu type {_mealOfTheDayTypeFakes.Lunch.Name},{_mealOfTheDayTypeFakes.Dinner.Name} to complete your menu."));
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

        var sut = new DefaultMenuCalculationStrategy(Resources);
        var result = sut.Calculate(menuCalendar, dishes);

        Assert.That(result.Value.Count() == 1);
        var calendarItem = result.Value.FirstOrDefault()!;
        Assert.That(calendarItem.Date, Is.EqualTo(_timePeriodFakes.OneDay.StartDate));
        Assert.That(calendarItem.Date, Is.EqualTo(_timePeriodFakes.OneDay.EndDate));
        Assert.That(calendarItem.MealOfTheDayTypes.Count(), Is.EqualTo(1));
        var calendarMealOfTheDayType = calendarItem.MealOfTheDayTypes.FirstOrDefault()!;
        Assert.That(calendarMealOfTheDayType.Id, Is.EqualTo(_mealOfTheDayTypeFakes.Breakfast.Id));
        Assert.That(calendarMealOfTheDayType.Dishes.Count(), Is.EqualTo(numberOfPeople));
        var dish = calendarMealOfTheDayType.Dishes.FirstOrDefault()!;
        Assert.That(dish.Id, Is.EqualTo(breakfastDish.Id));
    }

    /// <summary>
    ///  |----------Breakfast----------|------------Lunch------------|-----------Dinner------------|
    /// D|1.Person 1                   |1.Person 1                   |1.Person 1                   |
    /// a|  breakfastDish              |  lunchDish                  |  dinnerDish                 |
    /// y|2.Person 2                   |2.Person 2                   |2.Person 2                   |
    /// 1|  breakfastDish              |  lunchDish                  |  dinnerDish                 |
    /// -|----------Breakfast----------|------------Lunch------------|-----------Dinner------------|
    /// D|1.Person 1                   |1.Person 1                   |1.Person 1                   |
    /// a|  breakfastDish              |  lunchDish                  |  dinnerDish                 |
    /// y|2.Person 2                   |2.Person 2                   |2.Person 2                   |
    /// 2|  breakfastDish              |  lunchDish                  |  dinnerDish                 |
    /// -|-----------------------------|-----------------------------|-----------------------------|
    /// </summary>
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

        int dishesServingsIndex = 0;
        var expectedServings = new[]
        {
            breakfastDish,
            breakfastDish,
            lunchDish,
            lunchDish,
            dinnerDish,
            dinnerDish,
            breakfastDish,
            breakfastDish,
            lunchDish,
            lunchDish,
            dinnerDish,
            dinnerDish
        };

        var sut = new DefaultMenuCalculationStrategy(Resources);
        var result = sut.Calculate(menuCalendar, dishes);

        Assert.That(result.Value.Count() == 2);
        foreach ( var calendarItem in result.Value )
        {
            Assert.That(_timePeriodFakes.TwoDays.IsInPeriod(calendarItem.Date));
            Assert.That(calendarItem.MealOfTheDayTypes.Count() == 3);
            foreach (var calendarMealOfTheDayType in calendarItem.MealOfTheDayTypes)
            {
                Assert.That(menuCalendar.MealOfTheDayTypes.Any(x => x.Id == calendarMealOfTheDayType.Id));
                Assert.That(calendarMealOfTheDayType.Dishes.Count() == 2);
                foreach (var dish in calendarMealOfTheDayType.Dishes)
                {
                    Assert.That(dishes.First(x => x.Id == dish.Id).Name.Value,
                        Is.EqualTo(expectedServings[dishesServingsIndex].Name.Value));
                    dishesServingsIndex++;
                }
            }
        }
    }

    /// <summary>
    ///  |------------Lunch------------|-----------Dinner------------|
    /// D|1.Person 1                   |1.Person 1                   |
    /// a|  lunchAndDinnerDish3Servings|  lunchAndDinnerDish3Servings|
    /// y|2.Person 2                   |2.Person 2                   |
    /// 1|  lunchAndDinnerDish3Servings|  lunchAndDinnerDish2Servings|
    /// -|------------Lunch------------|-----------Dinner------------|
    /// D|1.Person 1                   |1.Person 1                   |
    /// a|  lunchAndDinnerDish2Servings|  dinnerDish                 |
    /// y|2.Person 2                   |2.Person 2                   |
    /// 2|  lunchAndDinnerDish3Servings|  lunchAndDinnerDish3Servings|
    /// -|------------Lunch------------|-----------Dinner------------|
    /// D|1.Person 1                   |1.Person 1                   |
    /// a|  lunchAndDinnerDish3Servings|  lunchAndDinnerDish2Servings|
    /// y|2.Person 2                   |2.Person 2                   |
    /// 3|  lunchAndDinnerDish2Servings|  dinnerDish                 |
    /// -|-----------------------------|-----------------------------|
    /// </summary>                     
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

        var sut = new DefaultMenuCalculationStrategy(Resources);
        var result = sut.Calculate(menuCalendar, dishes);
        int dishesServingsIndex = 0;
        var expectedServings = new[]
        {
            lunchAndDinnerDish3Servings,
            lunchAndDinnerDish3Servings,
            lunchAndDinnerDish3Servings,
            lunchAndDinnerDish2Servings,
            lunchAndDinnerDish2Servings,
            lunchAndDinnerDish3Servings,
            dinnerDish,
            lunchAndDinnerDish3Servings,
            lunchAndDinnerDish3Servings,
            lunchAndDinnerDish2Servings,
            lunchAndDinnerDish2Servings,
            dinnerDish,
        };

        Assert.That(result.Value.Count() == 3);
        foreach (var calendarItem in result.Value)
        {
            Assert.That(_timePeriodFakes.ThreeDays.IsInPeriod(calendarItem.Date));
            Assert.That(calendarItem.MealOfTheDayTypes.Count() == 2);
            foreach (var calendarMealOfTheDayType in calendarItem.MealOfTheDayTypes)
            {
                Assert.That(menuCalendar.MealOfTheDayTypes.Any(x => x.Id == calendarMealOfTheDayType.Id));
                Assert.That(calendarMealOfTheDayType.Dishes.Count() == 2);
                foreach (var dish in calendarMealOfTheDayType.Dishes)
                {
                    Assert.That(dishes.First(x => x.Id == dish.Id).Name.Value,
                        Is.EqualTo(expectedServings[dishesServingsIndex].Name.Value));
                    dishesServingsIndex++;
                }
            }
        }
    }

    /// <summary>
    ///  |------------Lunch------------|-----------Dinner------------|
    /// D|1.Person 1                   |1.Person 1                   |
    /// a|  lunchAndDinnerDish1        |  lunchAndDinnerDish2        |
    /// y|2.Person 2                   |2.Person 2                   |
    /// 1|  lunchAndDinnerDish2        |  dinnerDish                 |
    /// -|-----------------------------|-----------------------------|
    /// </summary>
    [Test]
    public void Calculate_Should_Fill_Calendar_For_One_Day_With_Lunch_And_Dinner_For_Two_People ()
    {
        var lunchAndDinnerDish1 = _dishFakes.LunchAndDinner(TestsConstants.Dishes.LUNCH_AND_DINNER_DISH + "1", 1);
        var lunchAndDinnerDish2 = _dishFakes.LunchAndDinner(TestsConstants.Dishes.LUNCH_AND_DINNER_DISH + "2", 2);
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

        var sut = new DefaultMenuCalculationStrategy(Resources);
        var result = sut.Calculate(menuCalendar, dishes);
        int dishesServingsIndex = 0;
        var expectedServings = new[]
        {
            lunchAndDinnerDish1,
            lunchAndDinnerDish2,
            lunchAndDinnerDish2,
            dinnerDish
        };

        Assert.That(result.Value.Count(), Is.EqualTo(1));
        foreach (var calendarItem in result.Value)
        {
            Assert.That(_timePeriodFakes.ThreeDays.IsInPeriod(calendarItem.Date));
            Assert.That(calendarItem.MealOfTheDayTypes.Count(), Is.EqualTo(2));
            foreach (var calendarMealOfTheDayType in calendarItem.MealOfTheDayTypes)
            {
                Assert.That(menuCalendar.MealOfTheDayTypes.Any(x => x.Id == calendarMealOfTheDayType.Id));
                Assert.That(calendarMealOfTheDayType.Dishes.Count(), Is.EqualTo(2));
                foreach (var dish in calendarMealOfTheDayType.Dishes)
                {
                    Assert.That(dishes.First(x => x.Id == dish.Id).Name.Value,
                        Is.EqualTo(expectedServings[dishesServingsIndex].Name.Value));
                    dishesServingsIndex++;
                }
            }
        }
    }
}
