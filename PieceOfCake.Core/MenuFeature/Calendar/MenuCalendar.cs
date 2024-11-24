using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.ValueObjects;
using System.Collections;

namespace PieceOfCake.Core.MenuFeature.Calendar;
public class MenuCalendar : IEnumerable<KeyValuePair<DateOnly, IDictionary<MealOfTheDayType, Dish[]>>>
{
    private readonly Dictionary<DateOnly, IDictionary<MealOfTheDayType, Dish[]>> _calendar;

    public MenuCalendar (
        TimePeriod duration,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes)
    {
        _calendar = new Dictionary<DateOnly, IDictionary<MealOfTheDayType, Dish[]>>(duration.DaysDifference);
        foreach (var day in duration)
        {
            _calendar.Add(day, new Dictionary<MealOfTheDayType, Dish[]>(mealOfTheDayTypes.Count()));
            foreach (var mealType in mealOfTheDayTypes)
            {

                _calendar[day].Add(mealType, new Dish[numberOfPeople]);
            }
        }

        MealOfTheDayTypes = mealOfTheDayTypes.ToHashSet();
        NumberOfPeople = numberOfPeople;
    }

    public ushort NumberOfPeople { get; }

    public HashSet<MealOfTheDayType> MealOfTheDayTypes { get; }

    public Dish this[DateOnly date, MealOfTheDayType mealOfTheDayType, ushort personIndex]
    {
        get => _calendar[date][mealOfTheDayType][personIndex];
        set => _calendar[date][mealOfTheDayType][personIndex] = value;
    }

    public IEnumerable<CalendarItem> Calendar
    {
        get
        {
            var selection = _calendar.Select(kv => new CalendarItem
            {
                Date = kv.Key,
                MealOfTheDayTypes = kv.Value.Select(x => new MealOfTheDayTypeInCalendar()
                {
                    Id = x.Key.Id,
                    Dishes = x.Value.Select(y => new DishInCalendar()
                    {
                        Id = y.Id
                    })
                })
            });

            return selection;
        }
    }

    public IEnumerator<KeyValuePair<DateOnly, IDictionary<MealOfTheDayType, Dish[]>>> GetEnumerator ()
    {
        foreach (var kvPair in _calendar)
        {
            yield return kvPair;
        }
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
        return GetEnumerator();
    }
}
