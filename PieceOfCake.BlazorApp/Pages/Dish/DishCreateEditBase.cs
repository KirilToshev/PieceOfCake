using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages.Dish
{
    public abstract class DishCreateEditBase : ComponentBase
    {
        [Inject]
        protected IDishHttpService DishHttpService { get; set; }

        public DishVm Dish { get; set; } = new DishVm();

        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public bool IsLoading { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        public abstract Task HandleValidSubmit();
    }
}
