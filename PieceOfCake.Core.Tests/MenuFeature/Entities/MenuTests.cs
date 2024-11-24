using AutoFixture;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Entities;
using PieceOfCake.Core.MenuFeature.Enumerations;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Tests.MenuFeature.Entities;

public class MenuTests : TestsBase
{
    private IDishFakes _dishFakes;
    private IMealOfTheDayTypeFakes _mealOfTheDayTypeFakes;
    private Mock<IDishRepository> _dishRepoMock;

    public MenuTests ()
    {
        _dishFakes = GetRequiredService<IDishFakes>();
        _mealOfTheDayTypeFakes = GetRequiredService<IMealOfTheDayTypeFakes>();
        _dishRepoMock = new Mock<IDishRepository>();
    }

    [Test]
    public void Create_Should_Return_User_Error_If_StartDate_Is_Bigger_Than_EndDate ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(-1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast
        };

        var result = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"Start Date {startDate.ToShortDateString()} " +
            $"of a time period must be less than its End Date {endDate.ToShortDateString()}"));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_There_Are_No_MealTypes ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[] { };

        var result = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"It is impossible to have a menu without at " +
            $"least one meal type."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_MealTypes_Are_Not_Unique ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast,
            _mealOfTheDayTypeFakes.Breakfast
        };

        var result = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"Meals of the day must be unique."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_There_Are_No_People ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        ushort numberOfPeople = 0;
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast
        };

        var result = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"Menu should have one or more people."));
    }

    [Test]
    public void Create_Should_Succseed_If_Data_Is_Valid ()
    {
        var expectedDaysDifference = 7;
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(6);
        ushort numberOfPeople = 2;
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast,
            _mealOfTheDayTypeFakes.Lunch,
            _mealOfTheDayTypeFakes.Dinner
        };

        var result = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);

        Assert.That(result.IsSuccess);
        var menu = result.Value;
        Assert.That(menu.NumberOfPeople, Is.EqualTo(numberOfPeople));
        Assert.That(menu.Duration.StartDate, Is.EqualTo(DateOnly.FromDateTime(startDate)));
        Assert.That(menu.Duration.EndDate, Is.EqualTo(DateOnly.FromDateTime(endDate)));
        Assert.That(menu.Duration.DaysDifference, Is.EqualTo(expectedDaysDifference));
        Assert.That(menu.Type, Is.EqualTo(MenuType.None));
        Assert.That(menu.MealOfTheDayTypes, Is.EquivalentTo(mealTypes));
        Assert.That(menu.Calendar.Any() == false);
    }

    [Test]
    public void Update_Should_Return_User_Error_If_StartDate_Is_Bigger_Than_EndDate ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast
        };

        var menu = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);
        var result = menu.Value.Update(startDate, endDate.AddDays(-2), numberOfPeople, mealTypes, Resources);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"Start Date {startDate.ToShortDateString()} " +
            $"of a time period must be less than its End Date {endDate.AddDays(-2).ToShortDateString()}"));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_There_Are_No_MealTypes ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast
        };

        var menu = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);
        var result = menu.Value.Update(startDate, endDate, numberOfPeople, new MealOfTheDayType[] { }, Resources);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"It is impossible to have a menu without at " +
            $"least one meal type."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_MealTypes_Are_Not_Unique ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast
        };

        var menu = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);
        var result = menu.Value.Update(
            startDate,
            endDate,
            numberOfPeople,
            new MealOfTheDayType[]
            {
                _mealOfTheDayTypeFakes.Breakfast,
                _mealOfTheDayTypeFakes.Breakfast
            },
            Resources);


        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"Meals of the day must be unique."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_There_Are_No_People ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast
        };

        var menu = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);
        var result = menu.Value.Update(startDate, endDate, 0, mealTypes, Resources);


        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"Menu should have one or more people."));
    }

    [Test]
    public void Update_Should_Succseed_If_Data_Is_Valid ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast
        };
        var menu = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);

        var expectedDaysDifference = 3;
        var expectedStartDate = startDate.AddDays(1);
        var expectedEndDate = expectedStartDate.AddDays(2);
        var expectedNumberOfPeople = Fixture.Create<ushort>();
        var expectedMealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Dinner,
            _mealOfTheDayTypeFakes.Lunch,
        };

        var result = menu.Value.Update(
            expectedStartDate,
            expectedEndDate,
            expectedNumberOfPeople,
            expectedMealTypes,
            Resources);

        Assert.That(result.IsSuccess);
        var updatedMenu = result.Value;
        Assert.That(updatedMenu.NumberOfPeople, Is.EqualTo(expectedNumberOfPeople));
        Assert.That(updatedMenu.Duration.DaysDifference, Is.EqualTo(expectedDaysDifference));
        Assert.That(updatedMenu.Type, Is.EqualTo(MenuType.None));
        Assert.That(updatedMenu.MealOfTheDayTypes, Is.EquivalentTo(expectedMealTypes));
        Assert.That(updatedMenu.Calendar.Any() == false);
    }

    [Test]
    public async Task Update_Should_Clear_Calendar_Data ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        var numberOfPeople = Fixture.Create<ushort>();
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast
        };
        var menuResult = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources);
        var menu = menuResult.Value;
        _dishRepoMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Dish, bool>>>(), null))
            .ReturnsAsync(new Dish[] { _dishFakes.Breakfast() }.AsReadOnly());

        await menu.GenerateCalendar(_dishRepoMock.Object, Resources, CancellationToken.None);
        Assert.That(menu.Calendar is not null);
        Assert.That(menu.Calendar!.Any());

        var result = menu.Update(
            startDate,
            endDate,
            numberOfPeople,
            mealTypes,
            Resources);

        Assert.That(result.IsSuccess);
        Assert.That(menu.Calendar.Any() == false);
    }

    [Test]
    public async Task GenerateCalendar_Should_Succseed_To_Fill_In_Calendar ()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        ushort numberOfPeople = 2;
        var mealTypes = new MealOfTheDayType[]
        {
            _mealOfTheDayTypeFakes.Breakfast,
            _mealOfTheDayTypeFakes.Lunch
        };
        var dishes = new Dish[]
        {
            _dishFakes.Breakfast(),
            _dishFakes.Lunch()
        };
        _dishRepoMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Dish, bool>>>(), null))
            .ReturnsAsync(dishes.AsReadOnly());

        var menu = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources).Value;
        var result = await menu.GenerateCalendar(_dishRepoMock.Object, Resources, CancellationToken.None);

        Assert.That(result.IsSuccess);

        var dayOne = menu.Calendar.ToArray()[0];
        Assert.That(dayOne.Date, Is.EqualTo(DateOnly.FromDateTime(startDate)));
        var dayOneBreakfast = dayOne.MealOfTheDayTypes.ToArray()[0];
        var dayOneLunch = dayOne.MealOfTheDayTypes.ToArray()[1];
        Assert.That(dayOneBreakfast.Id, Is.EqualTo(_mealOfTheDayTypeFakes.Breakfast.Id));
        Assert.That(dayOneLunch.Id, Is.EqualTo(_mealOfTheDayTypeFakes.Lunch.Id));
        Assert.That(dayOneBreakfast.Dishes.Select(x => x.Id), 
            Is.EquivalentTo(new Guid[] { _dishFakes.Breakfast().Id, _dishFakes.Breakfast().Id }));
        Assert.That(dayOneLunch.Dishes.Select(x => x.Id), 
            Is.EquivalentTo(new Guid[] { _dishFakes.Lunch().Id, _dishFakes.Lunch().Id }));

        var dayTwo = menu.Calendar.ToArray()[1];
        Assert.That(dayTwo.Date, Is.EqualTo(DateOnly.FromDateTime(endDate)));
        var dayTwoBreakfast = dayTwo.MealOfTheDayTypes.ToArray()[0];
        var dayTwoLunch = dayTwo.MealOfTheDayTypes.ToArray()[1];
        Assert.That(dayTwoBreakfast.Id, Is.EqualTo(_mealOfTheDayTypeFakes.Breakfast.Id));
        Assert.That(dayTwoLunch.Id, Is.EqualTo(_mealOfTheDayTypeFakes.Lunch.Id));
        Assert.That(dayTwoBreakfast.Dishes.Select(x => x.Id),
            Is.EquivalentTo(new Guid[] { _dishFakes.Breakfast().Id, _dishFakes.Breakfast().Id }));
        Assert.That(dayTwoLunch.Dishes.Select(x => x.Id),
            Is.EquivalentTo(new Guid[] { _dishFakes.Lunch().Id, _dishFakes.Lunch().Id }));
    }
}
