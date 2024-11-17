namespace PieceOfCake.Application.DishFeature.Dtos;

public record MealOfTheDayTypeCalendarDto : MealOfTheDayTypeDto
{
    public required IEnumerable<DishInCalenderDto> Dishes { get; init; }
}
