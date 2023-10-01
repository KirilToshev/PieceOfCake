using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.ValueObjects;
using System.Collections;

namespace PieceOfCake.Core.MenuFeature.Calendar;
internal class MenuCalendar : IEnumerable<KeyValuePair<DateOnly, Dictionary<Guid, Dish[]>>>
{
    private readonly Dictionary<DateOnly, Dictionary<Guid, Dish[]>> _calendar;

    public MenuCalendar (
        TimePeriod duration,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes)
    {
        _calendar = new Dictionary<DateOnly, Dictionary<Guid, Dish[]>>(duration.DaysDifference);
        foreach (var day in duration)
        {
            _calendar.Add(day, new Dictionary<Guid, Dish[]>(mealOfTheDayTypes.Count()));
            foreach (var mealType in mealOfTheDayTypes)
            {

                _calendar[day].Add(mealType.Id, new Dish[numberOfPeople]);
            }
        }

        TotalDishCount = duration.DaysDifference * mealOfTheDayTypes.Count() * numberOfPeople;
        MealOfTheDayTypes = mealOfTheDayTypes.ToDictionary(x => x.Id);
    }

    public int TotalDishCount { get; }

    public IDictionary<Guid, MealOfTheDayType> MealOfTheDayTypes { get; }

    public Dish this[DateOnly dateIndex, Guid mealOfTheDayTypeIdIndex, ushort personIndex]
    {
        get => _calendar[dateIndex][mealOfTheDayTypeIdIndex][personIndex];
        set => _calendar[dateIndex][mealOfTheDayTypeIdIndex][personIndex] = value;
    }

    public IEnumerable<CalendarItem> Calendar
    {
        get
        {
            var selection = _calendar.Select(kv => new CalendarItem
            {
                Date = kv.Key,
                MealOfTheDayTypeDtos = kv.Value.Select(x => new MealOfTheDayTypeInCalendar()
                {
                    Id = x.Key,
                    Dishes = x.Value.Select(y => new DishInCalendar()
                    {
                        Id = y.Id
                    })
                })
            });

            return selection;
        }
    }

    public IEnumerator<KeyValuePair<DateOnly, Dictionary<Guid, Dish[]>>> GetEnumerator ()
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
