namespace PieceOfCake.Application.IngredientFeature.Dtos;

public record IngredientCreateCoreDto
{
    public required float Quantity { get; init; }

    public required Guid MeasureUnitId { get; init; }

    public required Guid ProductId { get; init; }
}
