using CSharpFunctionalExtensions;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using PieceOfCake.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services.Interfaces
{
    public interface IProductHttpService
    {
        Task<Result<IEnumerable<ProductVm>>> GetAllProducts();
        Task<Result<ProductVm>> GetProductById(long productId);
        Task<Result<ProductVm>> CreateProduct(ProductVm product);

        Task<Result<ProductVm>> UpdateProduct(ProductVm product);
        Task<Result> DeleteProduct(long productId);
    }
}
