using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Components
{
    public abstract class CreateEditBase<TItem> : ComponentBase
        where TItem: new()
    {
        public TItem Item { get; set; } = new TItem();

        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public bool IsLoading { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        public abstract Task HandleValidSubmit();
    }
}
