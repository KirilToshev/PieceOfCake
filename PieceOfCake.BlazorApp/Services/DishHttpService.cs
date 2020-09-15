using CSharpFunctionalExtensions;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Dish;
using PieceOfCake.Shared.ViewModels.Dish.Ingredient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services
{
    public class DishHttpService : HttpRequestServiceBase, IDishHttpService
    {
        public DishHttpService(HttpClient httpClient)
            :base(httpClient)
        {
        }

        public async Task<Result<IEnumerable<DishVm>>> GetAllDishes()
        {
            return await base.HandleGet<IEnumerable<DishVm>>($"api/dishes");
        }

        public async Task<Result<DishVm>> GetDishById(long dishId)
        {
            return await base.HandleGet<DishVm>($"api/dishes/{dishId}");
        }

        public async Task<Result<DishVm>> CreateDish(CreateDishVm dish)
        {
            return await base.HandlePost<DishVm>($"api/dishes", dish);
        }

        public async Task<Result<DishVm>> UpdateDish(UpdateDishVm dish)
        {
            return await base.HandlePut<DishVm>($"api/dishes/{dish.Id}", dish);
        }

        public async Task<Result> DeleteDish(long dishId)
        {
            return await base.HandleDelete($"api/dishes/{dishId}");
        }

        public async Task<Result<DishVm>> UpdateIngredients(long dishId, IEnumerable<AddIngredientVm> ingredients)
        {
            return await base.HandlePatch<DishVm>($"api/dishes/{dishId}", ingredients);
        }
    }
}
