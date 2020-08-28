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
    public abstract class DialogBase<T> : ComponentBase
        where T: new()
    {
        public bool ShowDialog { get; set; }

        public T Item { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        public void Show()
        {
            ResetDialog();
            ShowDialog = true;
            StateHasChanged();
        }

        public abstract string Title { get; }

        public abstract Task HandleValidSubmit();

        private void ResetDialog()
        {
            Item = new T();
            this.Errors = new List<string>();
        }

        protected async Task HandleValidSubmit(Func<T, Task<Result<T>>> serviceCallback)
        {
            this.Errors = new List<string>();
            var result = await serviceCallback(Item);

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
