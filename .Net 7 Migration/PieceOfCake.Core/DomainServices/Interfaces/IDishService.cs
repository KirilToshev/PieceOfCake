using CSharpFunctionalExtensions;
using PieceOfCake.Core.Dtos;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Core.DomainServices.Interfaces;

public interface IDishService : ICRUDService<Dish, Guid>
{
    Result<Dish> Create(
        string name, 
        string description, 
        int servingSize,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
    Result<Dish> Update(
        Guid id, 
        string name, 
        string description, 
        int servingSize,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsVmList);
}
