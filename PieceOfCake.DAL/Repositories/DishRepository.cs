
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.DAL.Repositories;

public class DishRepository(PocDbContext context) 
    : GenericRepository<Dish>(context), IDishRepository
{
}
