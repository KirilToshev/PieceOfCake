using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Product;

namespace PieceOfCake.BlazorApp.Components
{
    public class AddProductDialogBase : ProductDialogBase
    {
        public void Show()
        {
            ResetDialog();
            ShowDialog = true;
            StateHasChanged();
        }

        private void ResetDialog()
        {
            Product = new ProductVm();
            this.Errors = new List<string>();
        }

        protected async Task HandleValidSubmit()
        {
            await base.HandleValidSubmit(productService.CreateProduct);
        }
    }
}
