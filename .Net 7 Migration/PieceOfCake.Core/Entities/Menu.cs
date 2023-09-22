using CSharpFunctionalExtensions;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;

namespace PieceOfCake.Core.Entities;

public class Menu : Entity<Guid>
{
    protected Menu ()
    {
            
    }

    private Menu (
        TimePeriod duration,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes)
    {
        Duration = duration;
        
        Dishes = new HashSet<Dish>();
        NumberOfPeople = numberOfPeople;
        MealOfTheDayTypes = mealOfTheDayTypes;
    }

    public ushort NumberOfPeople { get; private set; }
    public TimePeriod Duration { get; private set; }
    public IEnumerable<MealOfTheDayType> MealOfTheDayTypes { get; private set; }
    public virtual ICollection<Dish> Dishes { get; protected set; }

    public static Result<Menu> Create(
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

    public Result<Menu> Update(
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

    public void ClearAllRelatedDishes()
    {
        this.Dishes.Clear();
    }

    public Result<Dictionary<DateOnly, ICollection<Dish>>> CalculateDishesPerDay(IEnumerable<Dish> dishes, IResources resources)
    {
        var result = new Dictionary<DateOnly, ICollection<Dish>>();
        if (!dishes.Any())
            return result;

        var totalNumberOfServings = MealOfTheDayTypes.Count() * Duration.DaysDifference * NumberOfPeople;

        if (dishes.Count() < totalNumberOfServings)
            return Result.Failure<Dictionary<DateOnly, ICollection<Dish>>>(resources.GenereteSentence(x => x.UserErrors.NotEnoughDishes));

        var index = 0;

        foreach (var day in Duration)
        {
            for (int i = 0; i < ServingsPerDay; i++, index++)
            {
                if (!result.ContainsKey(day))
                {
                    var dishesPerDayList = new List<Dish>();
                    result.Add(day, dishesPerDayList);
                }

                result[day].Add(dishes[index]);
            }
        }

        return result;
    }
}
