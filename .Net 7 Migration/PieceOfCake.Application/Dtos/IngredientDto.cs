namespace PieceOfCake.Application.Dtos;

public record IngredientDto
{
    public required float Quantity { get; init; }
    public required MeasureUnitDto MeasureUnit { get; init; }
    public required ProductDto Product { get; init; }
}
