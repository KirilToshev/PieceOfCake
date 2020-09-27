using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Dish;
using PieceOfCake.Shared.ViewModels.Dish.Ingredient;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using PieceOfCake.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages.Dish
{
    public class DishEditBase : CreateEditBase<DishVm>
    {
        [Inject]
        protected IDishHttpService DishHttpService { get; set; }

        [Inject]
        public IMeasureUnitHttpService MeasureUnitHttpService { get; set; }

        [Inject]
        public IProductHttpService ProdcutHttpService { get; set; }

        public IEnumerable<MeasureUnitVm> MeasureUnits { get; set; } = new HashSet<MeasureUnitVm>();

        public IEnumerable<ProductVm> Products { get; set; } = new HashSet<ProductVm>();

        public bool IsListUpdated { get; set; }

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

            this.Item = dishResult.Value;
            this.DisplayIngredientEditComponent = dishResult.Value.Ingredients.Select(x => false).ToArray();

            var measureUnitsList = await MeasureUnitHttpService.GetAllMeasureUnits();
            if (measureUnitsList.IsFailure)
                return;

            MeasureUnits = measureUnitsList.Value;

            var productsList = await ProdcutHttpService.GetAllProducts();
            if (productsList.IsFailure)
                return;

            Products = productsList.Value;
        }

        public override async Task HandleValidSubmit()
        {
            var updateModel = new UpdateDishVm()
            {
                Id = Item.Id,
                Name = Item.Name,
                Description = Item.Description
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
            var updatedList = Item.Ingredients.ToList();
            updatedList.Add(new ReadIngredientVm());
            Item.Ingredients = updatedList.ToArray();
            var updatedDisplayList = DisplayIngredientEditComponent.ToList();
            updatedDisplayList.Add(true);
            DisplayIngredientEditComponent = updatedDisplayList.ToArray();
            IsListUpdated = true;
            StateHasChanged();
        }

        public void EditIngredient(int index)
        {
            DisplayIngredientEditComponent[index] = false;
            StateHasChanged();
        }

        public void DeleteIngredient(ReadIngredientVm ingredient)
        {
            IsListUpdated = true;
            var updatedList = Item.Ingredients.ToList();
            var index = updatedList.FindIndex(0, x => x.Id == ingredient.Id);
            updatedList.Remove(ingredient);
            Item.Ingredients = updatedList;
            var updatedDisplayList = DisplayIngredientEditComponent.ToList();
            updatedDisplayList.RemoveAt(index);
            DisplayIngredientEditComponent = updatedDisplayList.ToArray();
            StateHasChanged();
        }

        public async Task UpdateIngredients()
        {
            IsListUpdated = true;
            var updateResult = await DishHttpService.UpdateIngredients(Item.Id, Item.Ingredients
                .Select(x => new AddIngredientVm()
                {
                    Quantity = x.Quantity,
                    MeasureUnitId = x.MeasureUnit.Id,
                    ProductId = x.Product.Id
                })).Finally(x => { IsListUpdated = false; return x; });

            if (updateResult.IsFailure)
                this.Errors = updateResult.Error.Split(';');
        }
    }
}
