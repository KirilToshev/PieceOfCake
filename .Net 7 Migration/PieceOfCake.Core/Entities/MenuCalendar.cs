using PieceOfCake.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieceOfCake.Core.Entities;
public class MenuCalendar
{
    private readonly Dictionary<DateOnly, Dictionary<Guid, Dish[]>> _dates;

    public MenuCalendar (
        TimePeriod duration, 
        ushort numberOfPeople, 
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes)
    {
        _dates = new Dictionary<DateOnly, Dictionary<Guid, Dish[]>>(duration.DaysDifference);
        foreach (var day in duration)
        {
            _dates.Add(day, new Dictionary<Guid, Dish[]>(mealOfTheDayTypes.Count()));
            foreach (var mealType in mealOfTheDayTypes)
            {
                _dates[day].Add(mealType.Id, new Dish[numberOfPeople]);
            }
        }
    }

    public Dish this[DateOnly dateIndex, Guid mealOfTheDayTypeIdIndex, ushort personIndex]
    {
        get => _dates[dateIndex][mealOfTheDayTypeIdIndex][personIndex];
        set => _dates[dateIndex][mealOfTheDayTypeIdIndex][personIndex] = value;
    }
}
