namespace PieceOfCake.Core.Dtos;

public record MealOfTheDayTypeCalendarDto : MealOfTheDayTypeDto
{
    public required IEnumerable<DishDto> Dishes { get; init; }
}
