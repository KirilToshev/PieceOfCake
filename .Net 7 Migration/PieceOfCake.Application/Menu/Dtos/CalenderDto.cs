using PieceOfCake.Application.MealOfTheDayType.Dtos;

namespace PieceOfCake.Application.Menu.Dtos;

public record CalenderDto
{
    public required DateOnly Date { get; init; }
    public required IEnumerable<MealOfTheDayTypeCalendarDto> MealOfTheDayTypeDtos { get; init; }
}