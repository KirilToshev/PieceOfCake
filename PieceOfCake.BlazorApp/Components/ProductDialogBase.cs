using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Components
{
    public abstract class ProductDialogBase : ComponentBase
    {
        public bool ShowDialog { get; set; }

        public ProductVm Product { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        [Inject]
        public IProductHttpService productService { get; set; }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        protected async Task HandleValidSubmit(Func<ProductVm, Task<Result<ProductVm>>> serviceCallback)
        {
            var result = await serviceCallback(Product);

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                return;
            }

            ShowDialog = false;

            await CloseEventCallback.InvokeAsync(true);
            StateHasChanged();
        }
    }
}
