namespace PieceOfCake.Application.DishFeature.Dtos;

public record MealOfTheDayTypeCalendarDto : MealOfTheDayTypeDto
{
    public required IEnumerable<DishDto> Dishes { get; init; }
}
