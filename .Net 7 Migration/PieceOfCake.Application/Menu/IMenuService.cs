using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common;

namespace PieceOfCake.Application.Menu;

public interface IMenuService : ICRUDService<Core.Menu.Menu, Guid>
{
    Result<Core.Menu.Menu> Create (DateTime startDate, DateTime endDate, ushort numberOfPeople, IEnumerable<Core.MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes);
    Result<Core.Menu.Menu> Update (Guid id, DateTime startDate, DateTime endDate, ushort numberOfPeople, IEnumerable<Core.MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes);
    Result<Core.Menu.Menu> GenerateDishesList (Guid id);
}