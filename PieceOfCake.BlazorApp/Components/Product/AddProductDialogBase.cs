using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Product;

namespace PieceOfCake.BlazorApp.Components.Product
{
    public class AddProductDialogBase : DialogBase<ProductVm>
    {
        [Inject]
        public IProductHttpService ProductService { get; set; }

        public override string Title => "Add Product";

        public override async Task HandleValidSubmit()
        {
            await base.HandleValidSubmit(ProductService.CreateProduct);
        }
    }
}
