using CSharpFunctionalExtensions;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Calendar;

namespace PieceOfCake.Core.MenuFeature.CalculationStrategies;
public interface IMenuCalculationStrategy
{
    Result<IEnumerable<CalendarItem>> Calculate (MenuCalendar calendar, IEnumerable<Dish> dishes);
}
