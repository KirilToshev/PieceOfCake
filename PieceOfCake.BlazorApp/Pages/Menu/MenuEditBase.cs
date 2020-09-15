using CSharpFunctionalExtensions;
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
    public class MenuEditBase : CreateEditBase<MenuVm>
    {
        [Parameter]
        public long Id { get; set; }

        [Inject]
        protected IMenuHttpService MenuHttpService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Errors = new List<string>();
            this.IsLoading = true;
            var menuResult = await MenuHttpService.GetMenuById(Id)
                .Finally(x =>
                {
                    this.IsLoading = false;
                    return x;
                });

            if (menuResult.IsFailure)
            {
                this.Errors = menuResult.Error.Split(';');
                return;
            }

            this.Item = menuResult.Value;
        }

        public override async Task HandleValidSubmit()
        {
            this.Errors = new List<string>();
            var updateResult = await this.MenuHttpService.Update(Item);
            if (updateResult.IsFailure)
            {
                this.Errors = updateResult.Error.Split(';');
                return;
            }   

            this.Item = updateResult.Value;
            StateHasChanged();
        }

        public async Task GenerateDishesList()
        {
            this.Errors = new List<string>();
            var dishesListResult = await MenuHttpService.GenerateDishesList(Id);
            if (dishesListResult.IsFailure)
            {
                this.Errors = dishesListResult.Error.Split(';');
                return;
            }   

            this.Item = dishesListResult.Value;
            StateHasChanged();
        }
    }
}
