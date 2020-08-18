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
    public class MeasureUnitHttpService : IMeasureUnitHttpService
    {
        private readonly HttpClient _httpClient;

        public MeasureUnitHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<MeasureUnitVm>> GetAllMeasureUnits()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<MeasureUnitVm>>
                (await _httpClient.GetStreamAsync($"api/measureunits"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<MeasureUnitVm> GetMeasureUnitById(int measureUnitId)
        {
            return await JsonSerializer.DeserializeAsync<MeasureUnitVm>
                (await _httpClient.GetStreamAsync($"api/measureunits/{measureUnitId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
