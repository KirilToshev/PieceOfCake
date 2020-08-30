using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Templates
{
    public partial class ConfirmationDialogTemplate
    {
        [Parameter]
        public bool ShowDialog { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        [Parameter]
        public EventCallback<bool> ConfirmEventCallback { get; set; }

        public async Task Close()
        {
            this.ShowDialog = false;
            await CloseEventCallback.InvokeAsync(true);
        }

        public async Task Confirm()
        {
            this.ShowDialog = false;
            await ConfirmEventCallback.InvokeAsync(true);
        }
    }
}
