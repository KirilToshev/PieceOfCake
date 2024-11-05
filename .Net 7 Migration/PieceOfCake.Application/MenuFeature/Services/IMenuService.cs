using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Entities;

namespace PieceOfCake.Application.MenuFeature.Services;

public interface IMenuService : IGetAndDeleteService<Menu, Guid>
{
    Result<Menu> Create (DateTime startDate, DateTime endDate, ushort numberOfPeople, IEnumerable<MealOfTheDayType> mealOfTheDayTypes);
    Result<Menu> Update (Guid id, DateTime startDate, DateTime endDate, ushort numberOfPeople, IEnumerable<MealOfTheDayType> mealOfTheDayTypes);
    Task<Result<Menu>> GenerateDishesList (Guid id);
}
