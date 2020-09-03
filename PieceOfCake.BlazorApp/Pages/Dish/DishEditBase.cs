using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Dish;
using PieceOfCake.Shared.ViewModels.Dish.Ingredient;
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

        public bool[] DisplayIngredientEditComponent { get; set; } = new bool[] { };

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
            this.DisplayIngredientEditComponent = dishResult.Value.Ingredients.Select(x => false).ToArray();
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

        public void AddIngredient()
        {
            var updatedList = Dish.Ingredients.ToList();
            updatedList.Add(new ReadIngredientVm());
            Dish.Ingredients = updatedList.ToArray();
            var updatedDisplayList = DisplayIngredientEditComponent.ToList();
            updatedDisplayList.Add(true);
            DisplayIngredientEditComponent = updatedDisplayList.ToArray();
            StateHasChanged();
        }

        public void EditIngredient(int index)
        {
            DisplayIngredientEditComponent[index] = false;
            StateHasChanged();
        }

        public void DeleteIngredient(ReadIngredientVm ingredient)
        {
            var updatedList = Dish.Ingredients.ToList();
            var index = updatedList.FindIndex(0, x => x.Id == ingredient.Id);
            updatedList.Remove(ingredient);
            Dish.Ingredients = updatedList;
            var updatedDisplayList = DisplayIngredientEditComponent.ToList();
            updatedDisplayList.RemoveAt(index);
            DisplayIngredientEditComponent = updatedDisplayList.ToArray();
            StateHasChanged();
        }
    }
}
