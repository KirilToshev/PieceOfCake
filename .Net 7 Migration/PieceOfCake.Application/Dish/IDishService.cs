using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common;
using PieceOfCake.Application.Dish.Dtos;

namespace PieceOfCake.Application.Dish;

public interface IDishService : ICRUDService<Core.Dish.Dish, Guid>
{
    Result<Core.Dish.Dish> Create (
        string name,
        string description,
        int servingSize,
        IEnumerable<Core.MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
    Result<Core.Dish.Dish> Update (
        Guid id,
        string name,
        string description,
        int servingSize,
        IEnumerable<Core.MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
}
