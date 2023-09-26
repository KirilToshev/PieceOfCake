using CSharpFunctionalExtensions;
using PieceOfCake.Application.Dtos;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Application.Services.Interfaces;

public interface IDishService : ICRUDService<Dish, Guid>
{
    Result<Dish> Create (
        string name,
        string description,
        int servingSize,
        IEnumerable<MealOfTheDayTypeDto> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
    Result<Dish> Update (
        Guid id,
        string name,
        string description,
        int servingSize,
        IEnumerable<MealOfTheDayTypeDto> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
}
