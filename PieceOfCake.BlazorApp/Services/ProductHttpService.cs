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
    public class ProductHttpService : HttpRequestServiceBase, IProductHttpService
    {
        public ProductHttpService(HttpClient httpClient)
            :base(httpClient)
        {
        }

        public async Task<Result<IEnumerable<ProductVm>>> GetAllProducts()
        {
            return await base.HandleGet<IEnumerable<ProductVm>>($"api/products");
        }

        public async Task<Result<ProductVm>> GetProductById(long productId)
        {
            return await base.HandleGet<ProductVm>($"api/products/{productId}");
        }

        public async Task<Result<ProductVm>> CreateProduct(ProductVm product)
        {
            return await base.HandlePost<ProductVm>($"api/products", product);
        }

        public async Task<Result<ProductVm>> UpdateProduct(ProductVm product)
        {
            return await base.HandlePut<ProductVm>($"api/products/{product.Id}", product);
        }

        public async Task<Result> DeleteProduct(long productId)
        {
            return await base.HandleDelete($"api/products/{productId}");
        }
    }
}
