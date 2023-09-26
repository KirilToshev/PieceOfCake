namespace PieceOfCake.Application.Dtos;

public record DishDto : IdNameDto<Guid>
{
    public required string Description { get; init; }
    public required int ServingSize { get; init; }
    public required IEnumerable<MealOfTheDayTypeDto> MealOfTheDayTypes { get; init; }
    public required string DishState { get; init; }
    public virtual required IReadOnlyCollection<IngredientDto> Ingredients { get; init; }
}
