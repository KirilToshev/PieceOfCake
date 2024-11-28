
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.DAL.Repositories;
public class ProductRepository(PocDbContext context) 
    : GenericRepository<Product>(context), IProductRepository
{
}
