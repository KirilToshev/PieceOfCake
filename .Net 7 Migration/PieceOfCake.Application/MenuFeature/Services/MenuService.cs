using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Application.MenuFeature.Services;

public class MenuService : IMenuService
{
    private readonly IResources _resources;
    private readonly IUnitOfWork _unitOfWork;

    public MenuService (
        IResources resources,
        IUnitOfWork unitOfWork)
    {
        _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public IReadOnlyCollection<Menu> Get () => _unitOfWork.MenuRepository.Get();

    public Result<Menu> Get (Guid id)
    {
        var menu = _unitOfWork.MenuRepository.GetById(id);

        if (menu == null)
            return Result.Failure<Menu>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return Result.Success(menu);
    }

    public Result<Menu> Create (
        DateTime startDate,
        DateTime endDate,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes)
    {
        return Menu.Create(startDate, endDate, numberOfPeople, mealOfTheDayTypes, _resources)
            .Tap(menu =>
            {
                _unitOfWork.MenuRepository.Insert(menu);
                _unitOfWork.Save();
            });
    }

    public Result<Menu> Update (
        Guid id,
        DateTime startDate,
        DateTime endDate,
        ushort numberOfPeople,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes)
    {
        var menuResult = Get(id);
        if (menuResult.IsFailure)
            return menuResult;

        var updateResult = menuResult.Value.Update(startDate, endDate, numberOfPeople, mealOfTheDayTypes, _resources);
        if (updateResult.IsFailure)
            return updateResult;

        _unitOfWork.MenuRepository.Update(menuResult.Value);
        _unitOfWork.Save();

        return Result.Success(menuResult.Value);
    }

    public Result Delete (Guid id)
    {
        return Get(id)
            .Tap(menu =>
            {
                _unitOfWork.MenuRepository.Delete(menu);
                _unitOfWork.Save();
            });
    }

    public Result<Menu> GenerateDishesList (Guid id)
    {
        //TODO: Implement generation of menu calendar.
        throw new NotImplementedException();
        //var menuResult = this.Get(id);
        //if (menuResult.IsFailure)
        //    return menuResult;

        //menuResult.Value.ClearAllRelatedDishes();
        //_unitOfWork.MenuRepository.Update(menuResult.Value);
        //_unitOfWork.Save();

        //var dishesListResult = menuResult.Value.GenerateDishesList(_unitOfWork, _resources);
        //if (dishesListResult.IsFailure)
        //    return dishesListResult.ConvertFailure<Menu>();

        //_unitOfWork.MenuRepository.Update(menuResult.Value);
        //_unitOfWork.Save();

        //return Result.Success(menuResult.Value);
    }
}
