using PieceOfCake.DTOs.Common;
using PieceOfCake.DTOs.IngredientFeature;

namespace PieceOfCake.DTOs.DishFeature;

public record DishDto : IdNameDto<Guid>
{
    public required string Description { get; init; }
    public required int ServingSize { get; init; }
    public required IEnumerable<MealOfTheDayTypeGetDto> MealOfTheDayTypes { get; init; }
    public required string DishState { get; init; }
    public required IEnumerable<IngredientDto> Ingredients { get; init; }
}
