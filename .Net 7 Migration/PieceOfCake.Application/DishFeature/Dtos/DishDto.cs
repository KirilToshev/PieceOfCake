using PieceOfCake.Application.Common.Dtos;
using PieceOfCake.Application.IngredientFeature.Dtos;

namespace PieceOfCake.Application.DishFeature.Dtos;

public record DishDto : IdNameDto<Guid>
{
    public required string Description { get; init; }
    public required int ServingSize { get; init; }
    public required IEnumerable<MealOfTheDayTypeDto> MealOfTheDayTypes { get; init; }
    public required string DishState { get; init; }
    public required IEnumerable<IngredientDto> Ingredients { get; init; }
}
