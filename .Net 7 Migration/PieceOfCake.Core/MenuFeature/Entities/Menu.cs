using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Calendar;
using PieceOfCake.Core.MenuFeature.Enumerations;
using PieceOfCake.Core.MenuFeature.Factories;
using PieceOfCake.Core.MenuFeature.ValueObjects;

namespace PieceOfCake.Core.MenuFeature.Entities;

public class Menu : GuidEntity
{
    private IEnumerable<Dish> _dishes;

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

    //TODO: MenuSummary Feature
    //public MenuSummary GetSummary(){}
    //Move TotalServingsCount and TotalDishesCounter into MenuSummary object 
    public int TotalServingsCount => Duration.DaysDifference * MealOfTheDayTypes.Count() * NumberOfPeople;
    public IDictionary<Guid, int>? TotalDishesCounter =>
        Calendar?.Select(c => c.MealOfTheDayTypes.ToList())
            .Aggregate((curr, next) => { curr.AddRange(next); return curr; })
            .Select(x => x.Dishes.ToList())
            .Aggregate((curr, next) => { curr.AddRange(next); return curr; })
            .GroupBy(kvPair => kvPair.Id)
            .ToDictionary(x => x.Key, x => x.Count());

    public virtual IReadOnlyCollection<Dish> Dishes => _dishes.ToList().AsReadOnly();

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

        if (!mealOfTheDayTypes.Any())
            return Result.Failure<Menu>(resources.GenereteSentence(x => x.UserErrors.MenuMustHaveAtLeastOneServing));

        if (mealOfTheDayTypes.HasUniqueValuesOnly())
            return Result.Failure<Menu>(resources.GenereteSentence(x => x.UserErrors.MealOfTheDayTypeAlreadyExists));

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
        ClearCalendar();

        return Result.Success(this);
    }

    public async Task<Result> GenerateCalendar(IDishRepository dishRepository, IResources resources, CancellationToken cancellationToken)
    {
        //TODO: Implement Specification Pattern
        //https://enterprisecraftsmanship.com/posts/cqrs-vs-specification-pattern/
        //TODO: Check this SQL Request !!!
        var dishes = await dishRepository
            .GetAsync(cancellationToken, 
                d => d.MealOfTheDayTypes
                .Where(mt => MealOfTheDayTypes.Contains(mt)).Any());

        var calendar = new MenuCalendar(Duration, NumberOfPeople, MealOfTheDayTypes);
        var calculationStrategy = MenuCalculationFactory.GetStrategy(Type, resources);
        var result = calculationStrategy.Calculate(calendar, dishes);

        if (result.IsSuccess)
            Calendar = result.Value;

        var dishesIds = Calendar.SelectMany(ci => ci.MealOfTheDayTypes.SelectMany(mt => mt.Dishes))
                                .Select(dish => dish.Id).Distinct();

        _dishes = dishes.Where(dish => dishesIds.Contains(dish.Id));

        return result;
    }

    public void ClearCalendar()
    {
        Calendar = Enumerable.Empty<CalendarItem>();
    }
}
