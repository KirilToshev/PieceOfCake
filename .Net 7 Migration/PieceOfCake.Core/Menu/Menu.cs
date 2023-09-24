using CSharpFunctionalExtensions;
using PieceOfCake.Core.Menu.ValueObjects;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Core.Menu;

public class Menu : Entity<Guid>
{
    protected Menu ()
    {

    }

    private Menu (
        TimePeriod duration,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes)
    {
        Duration = duration;

        Dishes = new HashSet<Dish.Dish>();
        NumberOfPeople = numberOfPeople;
        MealOfTheDayTypes = mealOfTheDayTypes;
    }

    public ushort NumberOfPeople { get; private set; }
    public TimePeriod Duration { get; private set; }
    public IEnumerable<MealOfTheDayType.MealOfTheDayType> MealOfTheDayTypes { get; private set; }
    public virtual ICollection<Dish.Dish> Dishes { get; protected set; }

    public static Result<Menu> Create (
        DateTime startDate,
        DateTime endDate,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes,
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
        IEnumerable<MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes,
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

    public void ClearAllRelatedDishes ()
    {
        Dishes.Clear();
    }


}
