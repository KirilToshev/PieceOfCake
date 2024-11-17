using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Application.DishFeature.Dtos.Mapping;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Application.DishFeature.Services;

public class MealOfTheDayTypeService : 
    BaseService<IMealOfTheDayTypeRepository, MealOfTheDayType>, 
    IMealOfTheDayTypeService
{
    protected override IMealOfTheDayTypeRepository Repository => 
        UnitOfWork.MealOfTheDayTypeRepository;

    public MealOfTheDayTypeService (
        IResources i18n,
        IUnitOfWork unitOfWork)
        : base (i18n, unitOfWork)
    {
    }

    public async Task<IReadOnlyCollection<MealOfTheDayTypeDto>> GetAllAsync ()
    {
        var mealTypes = await Repository.GetAsync();
        return mealTypes.Select(x => x.MapToGetDto()).ToArray().AsReadOnly();
    }

    public Task<Result<MealOfTheDayTypeDto>> GetByIdAsync (Guid id)
    {
        return GetEntityAsync(id).Map(x => x.MapToGetDto());
    }

    public Task<Result<MealOfTheDayTypeDto>> UpdateAsync (MealOfTheDayTypeUpdateDto mealOfTheDayTypeUpdateDto)
    {
        return GetEntityAsync(mealOfTheDayTypeUpdateDto.Id)
            .Bind(mt => mt.Update(mealOfTheDayTypeUpdateDto.Name, I18N, UnitOfWork)
            .Map(async mealOfTheDayType =>
            {
                Repository.Update(mealOfTheDayType);
                await SaveAsync();
                return mealOfTheDayType.MapToGetDto();
            }));
    }
    public Task<Result<MealOfTheDayTypeDto>> CreateAsync (MealOfTheDayTypeCreateDto mealOfTheDayTypeCreateDto)
    {
        return MealOfTheDayType.Create(mealOfTheDayTypeCreateDto.Name, I18N, UnitOfWork)
            .Map(async x =>
            {
                Repository.Insert(x);
                await SaveAsync();
                return x.MapToGetDto();
            });
    }

    public Task<Result> DeleteAsync (Guid id)
    {
        return GetEntityAsync(id)
            .Bind(async mu =>
            {
                //TODO: Implement specification pattern
                var usedInDishesList = await UnitOfWork.DishRepository
                                        .GetAsync(dish => dish.MealOfTheDayTypes.Select(x => x.Id).Contains(id));
                var usedInMenusList = await UnitOfWork.MenuRepository
                                        .GetAsync(menu => menu.MealOfTheDayTypes.Select(x => x.Id).Contains(id));

                if (usedInDishesList.Any() || usedInMenusList.Any())
                    return Result.Failure(I18N
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.MealOfTheDayType));

                Repository.Delete(mu);
                await UnitOfWork.SaveAsync();
                return Result.Success();
            });
    }
}
