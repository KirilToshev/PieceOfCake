using CSharpFunctionalExtensions;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services
{
    public class MeasureUnitHttpService : HttpRequestServiceBase<MeasureUnitVm>, IMeasureUnitHttpService
    {
        public MeasureUnitHttpService(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public async Task<Result<IEnumerable<MeasureUnitVm>>> GetAllMeasureUnits()
        {
            return await base.HandleGet<IEnumerable<MeasureUnitVm>>($"api/measureunits");
        }

        public async Task<Result<MeasureUnitVm>> GetMeasureUnitById(int measureUnitId)
        {
            return await base.HandleGet<MeasureUnitVm>($"api/measureunits/{measureUnitId}");
        }
    }
}
