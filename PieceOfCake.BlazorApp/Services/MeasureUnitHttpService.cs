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
    public class MeasureUnitHttpService : HttpRequestServiceBase, IMeasureUnitHttpService
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

        public async Task<Result<MeasureUnitVm>> CreateMeasureUnit(MeasureUnitVm measureUnit)
        {
            return await base.HandlePost<MeasureUnitVm>($"api/measureunits", measureUnit);
        }

        public async Task<Result<MeasureUnitVm>> UpdateMeasureUnit(MeasureUnitVm measureUnit)
        {
            return await base.HandlePut<MeasureUnitVm>($"api/measureunits/{measureUnit.Id}", measureUnit);
        }

        public async Task<Result> DeleteMeasureUnit(long measureUnitId)
        {
            return await base.HandleDelete($"api/measureunits/{measureUnitId}");
        }
    }
}
