using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Components;
using PieceOfCake.BlazorApp.Components.MeasureUnit;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages.MeasureUnit
{
    public class MeasureUnitListBase : ComponentBase
    {
        [Inject]
        public IMeasureUnitHttpService MeasureUnitHttpService { get; set; }

        public List<MeasureUnitVm> MeasureUnits { get; set; } = new List<MeasureUnitVm>();

        public List<string> Errors { get; set; } = new List<string>();

        public bool IsLoading { get; set; }

        protected AddMeasureUnitDialog AddMeasureUnitDialog { get; set; }

        protected EditMeasureUnitDialog EditMeasureUnitDialog { get; set; }

        protected ConfirmationDialog<MeasureUnitVm> DeleteConfirmationDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Dialog_OnDialogClose();
        }

        public async Task Dialog_OnDialogClose()
        {
            IsLoading = true;
            var result = await MeasureUnitHttpService.GetAllMeasureUnits().Finally(x =>
            {
                IsLoading = false;
                return x;
            });

            if (result.IsFailure)
            {
                Errors = result.Error.Split(';').ToList();
                return;
            }

            MeasureUnits = result.Value.ToList();

            StateHasChanged();
        }

        protected void AddProduct()
        {
            AddMeasureUnitDialog.Show();
            this.Errors = new List<string>();
        }

        protected void EditProduct(MeasureUnitVm measureUnit)
        {
            EditMeasureUnitDialog.Show(measureUnit);
            this.Errors = new List<string>();
        }

        public void ShowDeleteConfirmationDialog(MeasureUnitVm measureUnit)
        {
            DeleteConfirmationDialog.Show(measureUnit);
            this.Errors = new List<string>();
        }

        protected async Task DeleteProduct(MeasureUnitVm measureUnit)
        {
            IsLoading = true;
            var result = await MeasureUnitHttpService.DeleteMeasureUnit(measureUnit.Id).Finally(x =>
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

            this.MeasureUnits.Remove(measureUnit);

            StateHasChanged();
        }
    }
}
