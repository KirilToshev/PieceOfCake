using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages.Dish
{
    public class DishEditBase : DishCreateEditBase
    {
        [Parameter]
        public long Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.IsLoading = true;
            var dishResult = await DishHttpService.GetDishById(Id)
                .Finally(x =>
                {
                    this.IsLoading = false;
                    return x;
                });

            if (dishResult.IsFailure)
            {
                this.Errors = dishResult.Error.Split(';');
                return;
            }

            this.Dish = dishResult.Value;
        }

        public override async Task HandleValidSubmit()
        {
            var updateModel = new UpdateDishVm()
            {
                Id = Dish.Id,
                Name = Dish.Name,
                Description = Dish.Description
            };

            var updateResult = await this.DishHttpService.UpdateDish(updateModel);
            if (updateResult.IsFailure)
            {
                this.Errors = updateResult.Error.Split(';');
                return;
            }

            Navigation.NavigateTo("/dishes");
        }
    }
}
