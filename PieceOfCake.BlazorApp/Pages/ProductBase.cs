using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
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

        public List<MeasureUnitVm> Products { get; set; } = new List<MeasureUnitVm>();

        public List<string> Errors { get; set; } = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            var result = await ProductHttpService.GetAllProducts();
            var mu = await ProductHttpService.GetProductById(1);
            
        }
    }
}
