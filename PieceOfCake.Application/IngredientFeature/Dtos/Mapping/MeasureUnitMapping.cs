using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Application.IngredientFeature.Dtos.Mapping;

public static class MeasureUnitMapping
{
    public static MeasureUnitGetCoreDto MapToGetDto (this MeasureUnit measureUnit)
    {
        return new MeasureUnitGetCoreDto
        {
            Id = measureUnit.Id,
            Name = measureUnit.Name
        };
    }
}
