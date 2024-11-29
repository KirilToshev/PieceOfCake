using PieceOfCake.Application.DishFeature.Dtos;

namespace PieceOfCake.Application.MenuFeature.Dtos;
public record CalendarItemCoreDto
{
    public required DateOnly Date { get; init; }
    public required IEnumerable<MealOfTheDayTypeCalendarCoreDto> MealOfTheDayTypeDtos { get; init; }
}
