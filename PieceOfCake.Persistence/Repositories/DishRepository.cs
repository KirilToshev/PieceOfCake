using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;

namespace PieceOfCake.Persistence.Repositories
{
    public class DishRepository : GenericRepository<Dish>, IDishRepository
    {
        public DishRepository(PocDbContext context) : base(context)
        {
        }
    }
}
