using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Persistence.Repositories
{
    public class MeasureUnitRepository : GenericRepository<MeasureUnit>, IMeasureUnitRepository
    {
        public MeasureUnitRepository(PocDbContext context) : base(context)
        {
        }
    }
}
