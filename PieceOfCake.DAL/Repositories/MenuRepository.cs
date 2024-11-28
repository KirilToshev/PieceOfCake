
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.MenuFeature.Entities;

namespace PieceOfCake.DAL.Repositories;

public class MenuRepository(PocDbContext context) 
    : GenericRepository<Menu>(context), IMenuRepository
{
}
