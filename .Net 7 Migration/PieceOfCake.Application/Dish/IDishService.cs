using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common;
using PieceOfCake.Core.Dtos;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Application.Dish;

public interface IDishService : ICRUDService<Core.Entities.Dish, Guid>
{
    Result<Core.Entities.Dish> Create (
        string name,
        string description,
        int servingSize,
        IEnumerable<Core.Entities.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
    Result<Core.Entities.Dish> Update (
        Guid id,
        string name,
        string description,
        int servingSize,
        IEnumerable<Core.Entities.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
}
