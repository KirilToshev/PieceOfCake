using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using PieceOfCake.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services
{
    public abstract class HttpRequestService
    {
        private readonly HttpClient _httpClient;

        public HttpRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<T>> Handle<T>(string url)
        {
            var responce = await _httpClient.GetAsync(url);
            var content = await responce.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Envelope<T>>(content);

            if (responce.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Result.Success<T>(result.Result);
            }
            else if (responce.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Result.Failure<T>(result.ErrorMessage);
            }
            else
            {
                //handle 500 here
                var contentAsString = await responce.Content.ReadAsStringAsync();
                throw new Exception(contentAsString);
            }
        }
    }
}
