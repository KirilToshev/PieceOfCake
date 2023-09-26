using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Application.Menu;

public interface IMenuService : ICRUDService<Core.Entities.Menu, Guid>
{
    Result<Core.Entities.Menu> Create (DateTime startDate, DateTime endDate, ushort numberOfPeople, IEnumerable<Core.Entities.MealOfTheDayType> mealOfTheDayTypes);
    Result<Core.Entities.Menu> Update (Guid id, DateTime startDate, DateTime endDate, ushort numberOfPeople, IEnumerable<Core.Entities.MealOfTheDayType> mealOfTheDayTypes);
    Result<Core.Entities.Menu> GenerateDishesList (Guid id);
}
