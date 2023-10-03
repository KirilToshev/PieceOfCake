using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Calendar;
using PieceOfCake.Core.MenuFeature.Enumerations;
using PieceOfCake.Core.MenuFeature.Factories;
using PieceOfCake.Core.MenuFeature.ValueObjects;

namespace PieceOfCake.Core.MenuFeature.Entities;

public class Menu : GuidEntity
{
    protected Menu ()
    {

    }

    private Menu (
        TimePeriod duration,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes,
        MenuType type = MenuType.None)
    {
        Duration = duration;
        NumberOfPeople = numberOfPeople;
        MealOfTheDayTypes = mealOfTheDayTypes;
        Type = type;
        Calendar = new List<CalendarItem> ();
    }

    public ushort NumberOfPeople { get; private set; }
    public TimePeriod Duration { get; private set; }
    public MenuType Type { get; private set; }
    public IEnumerable<MealOfTheDayType> MealOfTheDayTypes { get; private set; }
    public IEnumerable<CalendarItem> Calendar { get; private set; }
    
    public static Result<Menu> Create (
        DateTime startDate,
        DateTime endDate,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes,
        IResources resources)
    {
        var duration = TimePeriod.Create(startDate, endDate, resources);
        if (duration.IsFailure)
            return duration.ConvertFailure<Menu>();

        if (mealOfTheDayTypes.Count() < 1)
            return Result.Failure<Menu>(resources.GenereteSentence(x => x.UserErrors.MenuMustHaveAtLeastOneServing));

        if (numberOfPeople < 1)
            return Result.Failure<Menu>(resources.GenereteSentence(x => x.UserErrors.MenuMustHaveAtleastOnePerson));

        return Result.Success(new Menu(duration.Value, numberOfPeople, mealOfTheDayTypes));
    }

    public Result<Menu> Update (
        DateTime startDate,
        DateTime endDate,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes,
        IResources resources)
    {
        var menuResult = Create(startDate, endDate, numberOfPeople, mealOfTheDayTypes, resources);
        if (menuResult.IsFailure)
            return menuResult;

        MealOfTheDayTypes = menuResult.Value.MealOfTheDayTypes;
        Duration = menuResult.Value.Duration;
        NumberOfPeople = numberOfPeople;

        return Result.Success(this);
    }

    public Result GenerateCalendar(IEnumerable<Dish> dishes, IResources resources)
    {
        var calendar = new MenuCalendar(Duration, NumberOfPeople, MealOfTheDayTypes);
        var calculationStrategy = MenuCalculationFactory.Create(Type, resources);
        var result = calculationStrategy.Calculate(calendar, dishes);

        if (result.IsSuccess)
            Calendar = result.Value;

        return result;
    }
}
