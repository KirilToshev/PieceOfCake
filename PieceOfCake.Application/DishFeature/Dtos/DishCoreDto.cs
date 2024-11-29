using PieceOfCake.Application.Common.Dtos;
using PieceOfCake.Application.IngredientFeature.Dtos;

namespace PieceOfCake.Application.DishFeature.Dtos;

public record DishCoreDto : IdNameCoreDto<Guid>
{
    public required string Description { get; init; }
    public required int ServingSize { get; init; }
    public required IEnumerable<MealOfTheDayTypeCoreDto> MealOfTheDayTypes { get; init; }
    public required string DishState { get; init; }
    public required IEnumerable<IngredientCoreDto> Ingredients { get; init; }
}
