using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Application.DishFeature.Services;

public interface IDishService : IGetAndDeleteService<Dish, Guid>
{
    Result<Dish> Create (
        string name,
        string description,
        byte servingSize,
        IEnumerable<MealOfTheDayTypeDto> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
    Result<Dish> Update (
        Guid id,
        string name,
        string description,
        byte servingSize,
        IEnumerable<MealOfTheDayTypeDto> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
}
