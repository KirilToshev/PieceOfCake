using PieceOfCake.DTOs.DishFeature;

namespace PieceOfCake.DTOs.MenuFeature;

public record CalendarItemDto
{
    public required DateOnly Date { get; init; }
    public required IEnumerable<MealOfTheDayTypeCalendarDto> MealOfTheDayTypeDtos { get; init; }
}
