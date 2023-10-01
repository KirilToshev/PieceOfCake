using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Calendar;

namespace PieceOfCake.Core.MenuFeature.CalculationStrategies;
internal interface IMenuCalculationStrategy
{
    Result<IEnumerable<CalendarItem>> Calculate (MenuCalendar calendar, IEnumerable<Dish> dishes, IResources resources);
}
