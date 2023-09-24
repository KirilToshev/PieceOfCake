using PieceOfCake.Application.Common;

namespace PieceOfCake.Application.Product.Services;

public interface IProductService : ICRUDService<Core.Product.Product, Guid>
{
}