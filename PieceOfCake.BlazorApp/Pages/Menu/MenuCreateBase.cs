using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages.Menu
{
    public class MenuCreateBase : CreateEditBase<MenuVm>
    {
        [Inject]
        protected IMenuHttpService DishHttpService { get; set; }

        public override async Task HandleValidSubmit()
        {
            var updateResult = await this.DishHttpService.Create(Item);
            if (updateResult.IsFailure)
            {
                this.Errors = updateResult.Error.Split(';');
                return;
            }

            Navigation.NavigateTo($"/menu/edit/{updateResult.Value.Id}");
        }
    }
}
