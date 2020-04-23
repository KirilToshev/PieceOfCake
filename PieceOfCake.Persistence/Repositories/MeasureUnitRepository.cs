using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Persistence.Repositories
{
    public class MeasureUnitRepository : GenericRepository<MeasureUnit>, IMeasureUnitRepository
    {
        public MeasureUnitRepository(PocDbContext context) : base(context)
        {
        }
    }
}
