using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Templates
{
    public partial class ModalDialogTemplate<TItem>
    {
        [Parameter]
        public bool ShowDialog { get; set; }

        [Parameter]
        public List<string> Errors { get; set; } = new List<string>();

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public RenderFragment<TItem> ItemForm { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public async Task Close()
        {
            this.ShowDialog = false;
            await CloseEventCallback.InvokeAsync(true);
        }
    }
}
