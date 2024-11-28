
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.DAL.Repositories;
public class MealOfTheDayTypeRepository(PocDbContext context) 
    : GenericRepository<MealOfTheDayType>(context), IMealOfTheDayTypeRepository
{
}
