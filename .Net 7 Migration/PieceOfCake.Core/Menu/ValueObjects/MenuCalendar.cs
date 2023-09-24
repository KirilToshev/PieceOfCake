namespace PieceOfCake.Core.Menu.ValueObjects;

public class MenuCalendar
{
    private readonly Dictionary<DateOnly, Dictionary<Guid, Dish.Dish[]>> _calendar;
    private readonly TimePeriod _duration;
    private readonly ushort _numberOfPeople;
    private readonly Dictionary<Guid, MealOfTheDayType.MealOfTheDayType> _mealOfTheDayTypes;

    public MenuCalendar (
        TimePeriod duration,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes)
    {
        _calendar = new Dictionary<DateOnly, Dictionary<Guid, Dish.Dish[]>>(duration.DaysDifference);
        foreach (var day in duration)
        {
            _calendar.Add(day, new Dictionary<Guid, Dish.Dish[]>(mealOfTheDayTypes.Count()));
            foreach (var mealType in mealOfTheDayTypes)
            {

                _calendar[day].Add(mealType.Id, new Dish.Dish[numberOfPeople]);
            }
        }

        _duration = duration;
        _numberOfPeople = numberOfPeople;
        _mealOfTheDayTypes = mealOfTheDayTypes.ToDictionary(key => key.Id, value => value);
    }

    public Dish.Dish this[DateOnly dateIndex, Guid mealOfTheDayTypeIdIndex, ushort personIndex]
    {
        get => _calendar[dateIndex][mealOfTheDayTypeIdIndex][personIndex];
        set => _calendar[dateIndex][mealOfTheDayTypeIdIndex][personIndex] = value;
    }
}
