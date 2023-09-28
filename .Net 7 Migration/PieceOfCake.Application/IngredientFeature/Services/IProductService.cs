using PieceOfCake.Application.Common.Services;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Application.IngredientFeature.Services;

public interface IProductService : ICRUDService<Product, Guid>
{
}
