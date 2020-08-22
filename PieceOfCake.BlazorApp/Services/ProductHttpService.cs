using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using PieceOfCake.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services
{
    public class ProductHttpService : IProductHttpService
    {
        private readonly HttpClient _httpClient;

        public ProductHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<IEnumerable<ProductVm>>> GetAllProducts()
        {
            var responce = await _httpClient.GetAsync($"api/products");
            var test = await responce.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Envelope<List<ProductVm>>>(test);

            if (responce.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Result.Success<IEnumerable<ProductVm>>(result.Result);
            }
            else if(responce.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Result.Failure<IEnumerable<ProductVm>>(result.ErrorMessage);
            }
            else
            {
                //TODO: write to console;
                var contentAsString = await responce.Content.ReadAsStringAsync();
                throw new Exception(contentAsString);
            }
        }

        public async Task<Result<ProductVm>> GetProductById(int productId)
        {
            var responce = await _httpClient.GetAsync($"api/products/{productId}");
            var test = await responce.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Envelope<ProductVm>>(test);
                //new JsonSerializerSettings
                //{
                //    ContractResolver = new DefaultContractResolver
                //    {
                //        NamingStrategy = new CamelCaseNamingStrategy()
                //    },
                //    //ObjectCreationHandling = ObjectCreationHandling.Replace
                //});

            return result.Result;
        }
    }
}
