using PieceOfCake.Application.Common.Dtos;

namespace PieceOfCake.Application.IngredientFeature.Dtos;

public record IngredientDto : IdDto<Guid>
{
    public required float Quantity { get; init; }
    public required MeasureUnitGetDto MeasureUnit { get; init; }
    public required ProductGetDto Product { get; init; }
}
