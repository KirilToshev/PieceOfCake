using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;

namespace PieceOfCake.Persistence.Repositories
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        public MenuRepository(PocDbContext context) : base(context)
        {
        }
    }
}
