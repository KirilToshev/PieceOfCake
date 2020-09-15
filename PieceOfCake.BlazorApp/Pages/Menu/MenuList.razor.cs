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
    partial class MenuList
    {
        [Inject]
        public IMenuHttpService MenuHttpService { get; set; }

        public List<MenuVm> Menus { get; set; } = new List<MenuVm>();

        public List<string> Errors { get; set; } = new List<string>();

        public bool IsLoading { get; set; }

        protected ConfirmationDialog<MenuVm> DeleteConfirmationDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Dialog_OnDialogClose();
        }

        public async Task Dialog_OnDialogClose()
        {
            IsLoading = true;
            var result = await MenuHttpService.GetAllMenus().Finally(x =>
            {
                IsLoading = false;
                return x;
            });

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                return;
            }

            Menus = result.Value.ToList();

            StateHasChanged();
        }

        public void ShowDeleteConfirmationDialog(MenuVm menu)
        {
            DeleteConfirmationDialog.Show(menu);
            this.Errors = new List<string>();
        }

        protected async Task Delete(MenuVm menu)
        {
            IsLoading = true;
            var result = await MenuHttpService.Delete(menu.Id).Finally(x =>
            {
                IsLoading = false;
                return x;
            });

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                StateHasChanged();
                return;
            }

            this.Menus.Remove(menu);

            StateHasChanged();
        }
    }
}
