using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages.Dish
{
    public class DishCreateBase : CreateEditBase<DishVm>
    {
        [Inject]
        protected IDishHttpService DishHttpService { get; set; }

        public override async Task HandleValidSubmit()
        {
            var createModel = new CreateDishVm()
            {
                Name = Item.Name,
                Description = Item.Description
            };

            var updateResult = await this.DishHttpService.CreateDish(createModel);
            if (updateResult.IsFailure)
            {
                this.Errors = updateResult.Error.Split(';');
                return;
            }

            Navigation.NavigateTo($"/dishes/edit/{updateResult.Value.Id}");
        }
    }
}
