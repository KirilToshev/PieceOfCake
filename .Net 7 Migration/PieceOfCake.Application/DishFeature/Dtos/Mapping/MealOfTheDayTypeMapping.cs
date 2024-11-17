using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Application.DishFeature.Dtos.Mapping;
public static class MealOfTheDayTypeMapping
{
    public static MealOfTheDayTypeDto MapToGetDto(this MealOfTheDayType mealOfTheDayType)
    {
        return new MealOfTheDayTypeDto
        {
            Id = mealOfTheDayType.Id,
            Name = mealOfTheDayType.Name,
        };
    }
}
