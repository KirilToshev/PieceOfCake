using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Application.IngredientFeature.Dtos.Mapping;

public static class ProductMapping
{
    public static ProductGetDto MapToGetDto(this Product product)
    {
        return new ProductGetDto
        {
            Id = product.Id,
            Name = product.Name
        };
    }
}
