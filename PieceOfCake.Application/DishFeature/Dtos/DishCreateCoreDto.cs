﻿using PieceOfCake.Application.IngredientFeature.Dtos;

namespace PieceOfCake.Application.DishFeature.Dtos;

public record DishCreateCoreDto
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required byte ServingSize { get; init; }
    public IEnumerable<Guid> MealOfTheDayTypeIds { get; init; } = Enumerable.Empty<Guid>();
    public IEnumerable<IngredientCreateCoreDto> IngredientsDtos { get; init; } = Enumerable.Empty<IngredientCreateCoreDto>();
}