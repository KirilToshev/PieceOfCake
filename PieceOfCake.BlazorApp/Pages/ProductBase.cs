using CSharpFunctionalExtensions;
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

        public bool IsLoading { get; set; }

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
    }
}
