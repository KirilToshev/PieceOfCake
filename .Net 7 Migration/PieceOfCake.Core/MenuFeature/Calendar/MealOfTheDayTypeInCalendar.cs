using PieceOfCake.Core.Common.Entities;

namespace PieceOfCake.Core.MenuFeature.Calendar;

public class MealOfTheDayTypeInCalendar : GuidEntity
{
    public new Guid Id { get; init; }
    public required IEnumerable<DishInCalendar> Dishes { get; init; }
}
