using PieceOfCake.Application.DishFeature.Dtos;

namespace PieceOfCake.Application.MenuFeature.Dtos;
public record CalendarItemDto
{
    public required DateOnly Date { get; init; }
    public required IEnumerable<MealOfTheDayTypeCalendarDto> MealOfTheDayTypeDtos { get; init; }
}
