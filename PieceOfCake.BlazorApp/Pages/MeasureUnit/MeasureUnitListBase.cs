using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
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

        //protected AddProductDialog AddProductDialog { get; set; }

        //protected EditProductDialog EditProductDialog { get; set; }

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
            //AddProductDialog.Show();
        }

        protected void EditProduct(MeasureUnitVm product)
        {
            //EditProductDialog.Show(product);
        }

        protected async void DeleteProduct(long measureUnitId)
        {
            //IsLoading = true;
            //var result = await MeasureUnitHttpService.DeleteMeasureUnit(measureUnitId).Finally(x =>
            //{
            //    IsLoading = false;
            //    return x;
            //});

            //if (result.IsFailure)
            //{
            //    Errors = result.Error.Split(';').ToList();
            //    StateHasChanged();
            //    return;
            //}

            //this.MeasureUnits.Remove(this.MeasureUnits.Find(x => x.Id == measureUnitId));

            //StateHasChanged();
        }
    }
}
