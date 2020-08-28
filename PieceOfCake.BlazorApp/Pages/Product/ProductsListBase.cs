using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Components.Product;
using PieceOfCake.BlazorApp.Services.Interfaces;
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
        }

        protected void EditProduct(ProductVm product)
        {
            EditProductDialog.Show(product);
        }

        protected async void DeleteProduct(long productId)
        {
            IsLoading = true;
            var result = await ProductHttpService.DeleteProduct(productId).Finally(x =>
            {
                IsLoading = false;
                return x;
            });

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                return;
            }

            this.Products.Remove(this.Products.Find(x => x.Id == productId));

            StateHasChanged();
        }
    }
}
