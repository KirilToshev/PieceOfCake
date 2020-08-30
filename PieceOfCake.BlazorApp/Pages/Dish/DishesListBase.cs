using CSharpFunctionalExtensions;
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
    public class DishesListBase : ComponentBase
    {
        [Inject]
        public IDishHttpService DishHttpService { get; set; }

        public List<DishVm> Dishes { get; set; } = new List<DishVm>();

        public List<string> Errors { get; set; } = new List<string>();

        public bool IsLoading { get; set; }

        protected ConfirmationDialog<DishVm> DeleteConfirmationDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Dialog_OnDialogClose();
        }

        public async Task Dialog_OnDialogClose()
        {
            IsLoading = true;
            var result = await DishHttpService.GetAllDishes().Finally(x =>
            {
                IsLoading = false;
                return x;
            });

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                return;
            }

            Dishes = result.Value.ToList();

            StateHasChanged();
        }

        public void ShowDeleteConfirmationDialog(DishVm dish)
        {
            DeleteConfirmationDialog.Show(dish);
            this.Errors = new List<string>();
        }

        protected async Task DeleteDish(DishVm dish)
        {
            IsLoading = true;
            var result = await DishHttpService.DeleteDish(dish.Id).Finally(x =>
            {
                IsLoading = false;
                return x;
            });

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                StateHasChanged();
                return;
            }

            this.Dishes.Remove(dish);

            StateHasChanged();
        }
    }
}
