using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Application.IngredientFeature.Dtos.Mapping;

public static class ProductMapping
{
    public static ProductGetCoreDto MapToGetDto(this Product product)
    {
        return new ProductGetCoreDto
        {
            Id = product.Id,
            Name = product.Name
        };
    }
}
