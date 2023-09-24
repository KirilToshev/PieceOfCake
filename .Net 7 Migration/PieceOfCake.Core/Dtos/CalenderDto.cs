namespace PieceOfCake.Core.Dtos;
public record CalenderDto
{
    public required DateOnly Date { get; init; }
    public required IEnumerable<MealOfTheDayTypeCalendarDto> MealOfTheDayTypeDtos { get; init; }
}
