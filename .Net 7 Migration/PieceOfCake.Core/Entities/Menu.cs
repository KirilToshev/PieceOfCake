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
        byte servingsPerDay, 
        ushort numberOfPeople)
    {
        Duration = duration;
        ServingsPerDay = servingsPerDay;
        Dishes = new HashSet<Dish>();
        NumberOfPeople = numberOfPeople;
    }

    public byte ServingsPerDay { get; private set; }

    public ushort NumberOfPeople { get; private set; }

    public TimePeriod Duration { get; private set; }

    public virtual ICollection<Dish> Dishes { get; protected set; }

    public static Result<Menu> Create(
        DateTime startDate, 
        DateTime endDate, 
        byte servingsPerDay, 
        ushort numberOfPeople,
        IResources resources)
    {
        var duration = TimePeriod.Create(startDate, endDate, resources);
        if (duration.IsFailure)
            return duration.ConvertFailure<Menu>();

        if (servingsPerDay < 1)
            return Result.Failure<Menu>(resources.GenereteSentence(x => x.UserErrors.MenuMustHaveAtLeastOneServing));

        if (numberOfPeople < 1)
            return Result.Failure<Menu>(resources.GenereteSentence(x => x.UserErrors.MenuMustHaveAtleastOnePerson));

        return Result.Success(new Menu(duration.Value, servingsPerDay, numberOfPeople));
    }

    public Result<Menu> Update(DateTime startDate,
        DateTime endDate,
        byte servingsPerDay,
        ushort numberOfPeople,
        IResources resources)
    {
        var menuResult = Create(startDate, endDate, servingsPerDay, numberOfPeople, resources);
        if (menuResult.IsFailure)
            return menuResult;

        ServingsPerDay = servingsPerDay;
        Duration = menuResult.Value.Duration;
        NumberOfPeople = numberOfPeople;
        
        return Result.Success(this);
    }

    public Result GenerateDishesList(IUnitOfWork unitOfWork, IResources resources)
    {
        var totalNumberOfServings = ServingsPerDay * Duration.DaysDifference * NumberOfPeople;
        var dishesList = unitOfWork.DishRepository.Get();

        if (dishesList.Count() < totalNumberOfServings)
            return Result.Failure<IEnumerable<Dish>>(resources.GenereteSentence(x => x.UserErrors.NotEnoughDishes));

        this.Dishes = dishesList.Take(totalNumberOfServings).ToList();

        return Result.Success();
    }

    public void ClearAllRelatedDishes()
    {
        this.Dishes.Clear();
    }

    public Result<Dictionary<DateOnly, ICollection<Dish>>> CalculateDishesPerDay(IResources resources)
    {
        var result = new Dictionary<DateOnly, ICollection<Dish>>();
        var dishes = Dishes.ToArray();
        if (!dishes.Any())
            return result;

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
