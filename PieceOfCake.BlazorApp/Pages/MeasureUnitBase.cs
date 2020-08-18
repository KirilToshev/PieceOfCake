using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Pages
{
    public class MeasureUnitBase : ComponentBase
    {
        [Inject]
        public IMeasureUnitHttpService MeasureUnitHttpService { get; set; }

        public List<MeasureUnitVm> MeasureUnits { get; set; } = new List<MeasureUnitVm>();

        protected override async Task OnInitializedAsync()
        {
            MeasureUnits = (await MeasureUnitHttpService.GetAllMeasureUnits()).ToList();
        }
    }
}
