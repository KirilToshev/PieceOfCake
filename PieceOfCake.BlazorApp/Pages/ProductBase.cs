using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using PieceOfCake.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages
{
    public class ProductBase : ComponentBase
    {
        [Inject]
        public IProductHttpService ProductHttpService { get; set; }

        public List<ProductVm> Products { get; set; } = new List<ProductVm>();

        public List<string> Errors { get; set; } = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            var result = await ProductHttpService.GetAllProducts();
            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                return;
            }

            Products = result.Value.ToList();

            //var mu = await ProductHttpService.GetProductById(2);
            
        }
    }
}
