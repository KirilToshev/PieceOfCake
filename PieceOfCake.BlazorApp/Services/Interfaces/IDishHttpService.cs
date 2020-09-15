using CSharpFunctionalExtensions;
using PieceOfCake.Shared.ViewModels.Dish;
using PieceOfCake.Shared.ViewModels.Dish.Ingredient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services.Interfaces
{
    public interface IDishHttpService
    {
        Task<Result<IEnumerable<DishVm>>> GetAllDishes();
        Task<Result<DishVm>> GetDishById(long dishId);
        Task<Result<DishVm>> CreateDish(CreateDishVm dish);
        Task<Result<DishVm>> UpdateDish(UpdateDishVm dish);
        Task<Result<DishVm>> UpdateIngredients(long dishId, IEnumerable<AddIngredientVm> ingredients);
        Task<Result> DeleteDish(long dishId);
    }
}
