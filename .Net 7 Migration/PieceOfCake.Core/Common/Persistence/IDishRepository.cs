using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Core.Common.Persistence;

public interface IDishRepository : IGenericRepository<Dish>
{
    bool IsDishInUse (Guid id);
}
