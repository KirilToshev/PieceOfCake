using PieceOfCake.Application.Common.Services;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Application.DishFeature.Services;

public interface IMealOfTheDayTypeService : ICRUDNameService<MealOfTheDayType, Guid>
{
}
