namespace PieceOfCake.Core.Dtos;

public record AddIngredientDto
{
    public required float Quantity { get; init; }

    public required Guid MeasureUnitId { get; init; }

    public required Guid ProductId { get; init; }
}
