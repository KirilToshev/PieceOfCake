using PieceOfCake.Application.MeasureUnit.Dtos;
using PieceOfCake.Application.Product.Dtos;

namespace PieceOfCake.Application.Dish.Dtos;

public record IngredientDto
{
    public required float Quantity { get; init; }
    public required MeasureUnitDto MeasureUnit { get; init; }
    public required ProductDto Product { get; init; }
}
