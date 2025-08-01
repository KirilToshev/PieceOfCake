using PieceOfCake.DTOs.Common;

namespace PieceOfCake.DTOs.IngredientFeature;

public record IngredientDto : IdDto<Guid>
{
    public required float Quantity { get; init; }
    public required MeasureUnitGetDto MeasureUnit { get; init; }
    public required ProductGetDto Product { get; init; }
}
