using PieceOfCake.Application.Common.Dtos;

namespace PieceOfCake.Application.IngredientFeature.Dtos;

public record IngredientCoreDto : IdCoreDto<Guid>
{
    public required float Quantity { get; init; }
    public required MeasureUnitGetCoreDto MeasureUnit { get; init; }
    public required ProductGetCoreDto Product { get; init; }
}
