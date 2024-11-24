using PieceOfCake.Core.IngredientFeature.ValueObjects;

namespace PieceOfCake.Application.IngredientFeature.Dtos.Mapping;

public static class IngredientMapping
{
    public static IngredientDto MapToGetDto(this Ingredient ingredient)
    {
        return new IngredientDto
        {
            Quantity = ingredient.Quantity,
            Product = ingredient.Product.MapToGetDto(),
            MeasureUnit = ingredient.MeasureUnit.MapToGetDto()
        };
    }
}
