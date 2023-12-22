using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Application.IngredientFeature.Dtos.Mapping;

public static class MeasureUnitMapping
{
    public static MeasureUnitGetDto MapToGetDto (this MeasureUnit measureUnit)
    {
        return new MeasureUnitGetDto
        {
            Id = measureUnit.Id,
            Name = measureUnit.Name
        };
    }
}
