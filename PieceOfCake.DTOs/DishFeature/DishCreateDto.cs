using PieceOfCake.DTOs.IngredientFeature;

namespace PieceOfCake.DTOs.DishFeature;

public record DishCreateDto
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required byte ServingSize { get; init; }
    public IEnumerable<Guid> MealOfTheDayTypeIds { get; init; } = Enumerable.Empty<Guid>();
    public IEnumerable<IngredientCreateDto> Ingredients { get; init; } = Enumerable.Empty<IngredientCreateDto>();
}
