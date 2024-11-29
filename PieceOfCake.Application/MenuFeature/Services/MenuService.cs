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

    public async Task<IReadOnlyCollection<MenuGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var menus = await Repository.GetAsync(cancellationToken);
        var data = await GetMenuAdditionalData(cancellationToken, menus.ToArray());
        return menus.Select(x => x.MapToGetDto(data.mealTypes, data.dishes))
            .ToArray()
            .AsReadOnly();
    }

    public async Task<Result<MenuGetDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var menuResult = await GetEntityAsync(id, cancellationToken);
        if(menuResult.IsFailure)
            return menuResult.ConvertFailure<MenuGetDto>();
        var data = await GetMenuAdditionalData(cancellationToken, menuResult.Value);
        return menuResult.Value.MapToGetDto(data.mealTypes, data.dishes);
    }

    public async Task<Result<MenuGetDto>> CreateAsync(MenuCreateCoreDto createDto, CancellationToken cancellationToken)
    {
        //TODO: Implement Specification pattern and reuse it in update method.
        var mealTypes = await GetRelatedMealOfTheDayTypes(createDto.MealOfTheDayTypes, cancellationToken);
        return await Menu.Create(
            startDate: createDto.StartDate,
            endDate: createDto.EndDate,
            numberOfPeople: createDto.NumberOfPeople, 
            mealOfTheDayTypes: mealTypes,
            I18N)
            .Map(async menu =>
            {
                Repository.Insert(menu);
                await UnitOfWork.SaveAsync(cancellationToken);
                return menu.MapToGetDto(
                    mealTypes,
                    Enumerable.Empty<Dish>());
            });
    }

    public async Task<Result<MenuGetDto>> UpdateAsync (MenuUpdateCoreDto updateDto, CancellationToken cancellationToken)
    {
        var menuResult = await GetEntityAsync(updateDto.Id, cancellationToken);
        if (menuResult.IsFailure)
            return menuResult.ConvertFailure<MenuGetDto>();
        var menu = menuResult.Value;
        //TODO: Implement Specification pattern
        var mealTypes = await GetRelatedMealOfTheDayTypes(updateDto.MealOfTheDayTypes, cancellationToken);

        return await menu.Update(
            startDate: updateDto.StartDate, 
            endDate: updateDto.EndDate, 
            numberOfPeople: updateDto.NumberOfPeople, 
            mealOfTheDayTypes: mealTypes, 
            I18N)
            .Map(async updatedMenu =>
            {
                Repository.Update(updatedMenu);
                await UnitOfWork.SaveAsync(cancellationToken);
                return updatedMenu.MapToGetDto(
                    mealTypes,
                    Enumerable.Empty<Dish>());
            });
    }

    public async Task<Result> DeleteAsync (Guid id, CancellationToken cancellationToken)
    {
        return await GetEntityAsync(id, cancellationToken)
            .Tap(async menu =>
            {
                Repository.Delete(menu);
                await UnitOfWork.SaveAsync(cancellationToken);
            });
    }

    public async Task<Result<Menu>> GenerateDishesListAsync (Guid id, CancellationToken cancellationToken)
    {
        var menuResult = await GetEntityAsync(id, cancellationToken);
        if (menuResult.IsFailure)
            return menuResult;
        var menu = menuResult.Value;

        var result = await menu.GenerateCalendar(UnitOfWork.DishRepository, I18N, cancellationToken);
        if (result.IsFailure)
            return result.ConvertFailure<Menu>();

        Repository.Update(menuResult.Value);
        await UnitOfWork.SaveAsync(cancellationToken);

        return menuResult.Value;
    }

    private Task<IReadOnlyCollection<MealOfTheDayType>> GetRelatedMealOfTheDayTypes(IEnumerable<Guid> mealOfTheDayTypeDtos, CancellationToken cancellationToken)
    {
        return UnitOfWork.MealOfTheDayTypeRepository
            .GetAsync(cancellationToken, mt => mealOfTheDayTypeDtos.Contains(mt.Id));
    }

    private async Task<(
        IReadOnlyCollection<MealOfTheDayType> mealTypes
        , IReadOnlyCollection<Dish> dishes)> GetMenuAdditionalData(CancellationToken cancellationToken, params Menu[] menus)
    {
        // Why this can NOT be in parallel: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#aasync Tasking-dbcontext-threading-issues

        var allMealTypes = menus
            .SelectMany(m => m.Calendar.SelectMany(c => c.MealOfTheDayTypes));
        var distinctDisheIds = allMealTypes
            .SelectMany(mt => mt.Dishes.Select(dish => dish.Id)).Distinct();
        var mealTypes = await UnitOfWork.MealOfTheDayTypeRepository
            .GetAsync(cancellationToken, mt => allMealTypes.Select(m => m.Id).Contains(mt.Id));
        var dishes = await UnitOfWork.DishRepository
            .GetAsync(cancellationToken, dish => distinctDisheIds.Contains(dish.Id));
        return (mealTypes, dishes);
    }
}
