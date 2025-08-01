namespace PieceOfCake.DTOs.IngredientFeature;

public record IngredientCreateDto
{
    public required float Quantity { get; init; }

    public required Guid MeasureUnitId { get; init; }

    public required Guid ProductId { get; init; }
}
