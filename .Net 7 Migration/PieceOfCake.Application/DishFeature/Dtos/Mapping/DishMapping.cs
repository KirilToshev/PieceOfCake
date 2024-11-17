using PieceOfCake.Application.IngredientFeature.Dtos.Mapping;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Application.DishFeature.Dtos.Mapping;
public static class DishMapping
{
    public static DishDto MapToGetDto(this Dish entity)
    {
        return new DishDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ServingSize = entity.ServingSize,
            DishState = entity.DishState.State.ToString(),
            MealOfTheDayTypes = entity.MealOfTheDayTypes.Select(mt => mt.MapToGetDto()),
            Ingredients = entity.Ingredients.Select(i => i.MapToGetDto())
        };
    }
}
