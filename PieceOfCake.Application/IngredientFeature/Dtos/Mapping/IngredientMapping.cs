using PieceOfCake.Core.IngredientFeature.ValueObjects;

namespace PieceOfCake.Application.IngredientFeature.Dtos.Mapping;

public static class IngredientMapping
{
    public static IngredientCoreDto MapToGetDto(this Ingredient ingredient)
    {
        return new IngredientCoreDto
        {
            Id = ingredient.Id,
            Quantity = ingredient.Quantity,
            Product = ingredient.Product.MapToGetDto(),
            MeasureUnit = ingredient.MeasureUnit.MapToGetDto()
        };
    }
}
