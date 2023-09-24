using PieceOfCake.Application.Dish.Dtos;

namespace PieceOfCake.Application.MealOfTheDayType.Dtos;

public record MealOfTheDayTypeCalendarDto : MealOfTheDayTypeDto
{
    public required IEnumerable<DishDto> Dishes { get; init; }
}