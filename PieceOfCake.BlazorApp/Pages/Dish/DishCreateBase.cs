using PieceOfCake.Shared.ViewModels.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages.Dish
{
    public class DishCreateBase : DishCreateEditBase
    {
        public override async Task HandleValidSubmit()
        {
            var createModel = new CreateDishVm()
            {
                Name = Dish.Name,
                Description = Dish.Description
            };

            var updateResult = await this.DishHttpService.CreateDish(createModel);
            if (updateResult.IsFailure)
            {
                this.Errors = updateResult.Error.Split(';');
                return;
            }

            Navigation.NavigateTo($"/edit/{updateResult.Value.Id}");
        }
    }
}
