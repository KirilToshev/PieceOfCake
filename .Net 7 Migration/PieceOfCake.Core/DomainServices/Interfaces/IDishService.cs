using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.IoModels;

namespace PieceOfCake.Core.DomainServices.Interfaces;

public interface IDishService : ICRUDService<Dish, Guid>
{
    Result<Dish> Create(
        string name, 
        string description, 
        int servingSize,
        Guid mealOfTheDayType,
        IEnumerable<AddIngredientDto> ingredientsVmList);
    Result<Dish> Update(
        Guid id, 
        string name, 
        string description, 
        int servingSize,
        Guid mealOfTheDayType,
        IEnumerable<AddIngredientDto> ingredientsVmList);
}
