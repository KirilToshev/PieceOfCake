
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.DAL.Repositories;
public class MeasureUnitRepository(PocDbContext context) 
    : GenericRepository<MeasureUnit>(context), IMeasureUnitRepository
{
}
