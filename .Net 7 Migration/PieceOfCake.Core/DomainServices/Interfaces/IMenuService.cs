using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Core.DomainServices.Interfaces;

public interface IMenuService : ICRUDService<Menu, Guid>
{
    Result<Menu> Create(DateTime startDate, DateTime endDate, ushort numberOfPeople, IEnumerable<MealOfTheDayType> mealOfTheDayTypes);
    Result<Menu> Update(Guid id, DateTime startDate, DateTime endDate, ushort numberOfPeople, IEnumerable<MealOfTheDayType> mealOfTheDayTypes);
    Result<Menu> GenerateDishesList(Guid id);
}
