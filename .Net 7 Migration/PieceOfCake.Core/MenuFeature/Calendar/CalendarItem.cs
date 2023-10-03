namespace PieceOfCake.Core.MenuFeature.Calendar;
public class CalendarItem
{
    public required DateOnly Date { get; init; }
    public required IEnumerable<MealOfTheDayTypeInCalendar> MealOfTheDayTypes { get; init; }
}
