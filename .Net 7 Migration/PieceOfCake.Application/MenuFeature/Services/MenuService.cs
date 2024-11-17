using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.MenuFeature.Dtos;
using PieceOfCake.Application.MenuFeature.Dtos.Mapping;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Entities;

namespace PieceOfCake.Application.MenuFeature.Services;

public class MenuService : BaseService<IMenuRepository, Menu>, IMenuService
{
    protected override IMenuRepository Repository => UnitOfWork.MenuRepository;

    public MenuService (
        IResources resources,
        IUnitOfWork unitOfWork)
        : base(resources, unitOfWork)
    {
    }

    public async Task<IReadOnlyCollection<MenuGetDto>> GetAllAsync()
    {
        var menus = await Repository.GetAsync();
        var data = await GetMenuAdditionalData(menus.ToArray());
        return menus.Select(x => x.MapToGetDto(data.mealTypes, data.dishes))
            .ToArray()
            .AsReadOnly();
    }
    public async Task<Result<MenuGetDto>> GetByIdAsync(Guid id)
    {
        var menuResult = await GetEntityAsync(id);
        if(menuResult.IsFailure)
            return menuResult.ConvertFailure<MenuGetDto>();
        var data = await GetMenuAdditionalData(menuResult.Value);
        return menuResult.Value.MapToGetDto(data.mealTypes, data.dishes);
    }

    public Task<Result<MenuGetDto>> CreateAsync(MenuCreateDto createDto)
    {
        return Menu.Create(startDate, endDate, numberOfPeople, mealOfTheDayTypes, _resources)
            .Tap(menu =>
            {
                _unitOfWork.MenuRepository.Insert(menu);
                _unitOfWork.Save();
            });
    }

    public Task<Result<MenuGetDto>> UpdateAsync (MenuUpdateDto updateDto)
    {
        var menuResult = GetByIdAsync(id);
        if (menuResult.IsFailure)
            return menuResult;
        var menu = menuResult.Value;

        var updateResult = menu.Update(startDate, endDate, numberOfPeople, mealOfTheDayTypes, _resources);
        if (updateResult.IsFailure)
            return updateResult;

        _unitOfWork.MenuRepository.Update(menu);
        _unitOfWork.Save();

        return Result.Success(menu);
    }

    public async Task<Result> DeleteAsync (Guid id)
    {
        return await GetEntityAsync(id)
            .Tap(async menu =>
            {
                Repository.Delete(menu);
                await UnitOfWork.SaveAsync();
            });
    }

    public async Task<Result<Menu>> GenerateDishesList (Guid id)
    {
        var menuResult = await GetEntityAsync(id);
        if (menuResult.IsFailure)
            return menuResult;
        var menu = menuResult.Value;

        var result = await menu.GenerateCalendar(UnitOfWork.DishRepository, I18N);
        if (result.IsFailure)
            return result.ConvertFailure<Menu>();

        Repository.Update(menuResult.Value);
        await UnitOfWork.SaveAsync();

        return menuResult.Value;
    }

    private async Task<(
        IReadOnlyCollection<MealOfTheDayType> mealTypes
        , IReadOnlyCollection<Dish> dishes)> GetMenuAdditionalData(params Menu[] menus)
    {
        // Why this can NOT be in parallel: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#avoiding-dbcontext-threading-issues

        var allMealTypes = menus
            .SelectMany(m => m.Calendar.SelectMany(c => c.MealOfTheDayTypes));
        var distinctDisheIds = allMealTypes
            .SelectMany(mt => mt.Dishes.Select(dish => dish.Id)).Distinct();
        var mealTypes = await UnitOfWork.MealOfTheDayTypeRepository
            .GetAsync(mt => allMealTypes.Select(m => m.Id).Contains(mt.Id));
        var dishes = await UnitOfWork.DishRepository
            .GetAsync(dish => distinctDisheIds.Contains(dish.Id));
        return (mealTypes, dishes);
    }
}
