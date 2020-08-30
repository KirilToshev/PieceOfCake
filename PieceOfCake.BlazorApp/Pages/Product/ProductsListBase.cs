using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Components;
using PieceOfCake.BlazorApp.Components.Product;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.BlazorApp.Templates;
using PieceOfCake.Shared.ViewModels.Product;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages.Product
{
    public class ProductsListBase : ComponentBase
    {
        [Inject]
        public IProductHttpService ProductHttpService { get; set; }

        public List<ProductVm> Products { get; set; } = new List<ProductVm>();

        public List<string> Errors { get; set; } = new List<string>();

        public bool IsLoading { get; set; }

        protected AddProductDialog AddProductDialog { get; set; }

        protected EditProductDialog EditProductDialog { get; set; }

        protected ConfirmationDialog<ProductVm> ConfirmationDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            var result = await ProductHttpService.GetAllProducts().Finally(x =>
            {
                IsLoading = false;
                return x;
            });

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                return;
            }

            Products = result.Value.ToList();
        }

        public async void AddProductDialog_OnDialogClose()
        {
            IsLoading = true;
            var result = await ProductHttpService.GetAllProducts().Finally(x =>
            {
                IsLoading = false;
                return x;
            });

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                return;
            }

            Products = result.Value.ToList();

            StateHasChanged();
        }

        protected void AddProduct()
        {
            AddProductDialog.Show();
            this.Errors = new List<string>();
        }

        protected void EditProduct(ProductVm product)
        {
            EditProductDialog.Show(product);
            this.Errors = new List<string>();
        }

        public void ShowDeleteConfirmationDialog(ProductVm product)
        {
            ConfirmationDialog.Show(product);
            this.Errors = new List<string>();
        }

        public async Task DeleteProduct(ProductVm product)
        {
            IsLoading = true;
            var result = await ProductHttpService.DeleteProduct(product.Id).Finally(x =>
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

            this.Products.Remove(product);

            StateHasChanged();
        }
    }
}
