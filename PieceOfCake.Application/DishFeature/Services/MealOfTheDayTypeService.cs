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

    public async Task<IReadOnlyCollection<MealOfTheDayTypeDto>> GetAllAsync (CancellationToken cancellationToken)
    {
        var mealTypes = await Repository.GetAsync(cancellationToken);
        return mealTypes.Select(x => x.MapToGetDto()).ToArray().AsReadOnly();
    }

    public Task<Result<MealOfTheDayTypeDto>> GetByIdAsync (Guid id, CancellationToken cancellationToken)
    {
        return GetEntityAsync(id, cancellationToken).Map(x => x.MapToGetDto());
    }

    public Task<Result<MealOfTheDayTypeDto>> UpdateAsync (MealOfTheDayTypeUpdateDto mealOfTheDayTypeUpdateDto, CancellationToken cancellationToken)
    {
        return GetEntityAsync(mealOfTheDayTypeUpdateDto.Id, cancellationToken)
            .Bind(mt => mt.UpdateAsync(mealOfTheDayTypeUpdateDto.Name, I18N, UnitOfWork, cancellationToken)
            .Map(async mealOfTheDayType =>
            {
                Repository.Update(mealOfTheDayType);
                await SaveAsync(cancellationToken);
                return mealOfTheDayType.MapToGetDto();
            }));
    }
    public Task<Result<MealOfTheDayTypeDto>> CreateAsync (MealOfTheDayTypeCreateDto mealOfTheDayTypeCreateDto, CancellationToken cancellationToken)
    {
        return MealOfTheDayType.Create(mealOfTheDayTypeCreateDto.Name, I18N, UnitOfWork, cancellationToken)
            .Map(async x =>
            {
                Repository.Insert(x);
                await SaveAsync(cancellationToken);
                return x.MapToGetDto();
            });
    }

    public Task<Result> DeleteAsync (Guid id, CancellationToken cancellationToken)
    {
        return GetEntityAsync(id, cancellationToken)
            .Bind(async mu =>
            {
                //TODO: Implement specification pattern
                var usedInDishesList = await UnitOfWork.DishRepository
                                        .GetAsync(cancellationToken, dish => dish.MealOfTheDayTypes.Select(x => x.Id).Contains(id));
                var usedInMenusList = await UnitOfWork.MenuRepository
                                        .GetAsync(cancellationToken, menu => menu.MealOfTheDayTypes.Select(x => x.Id).Contains(id));

                if (usedInDishesList.Any() || usedInMenusList.Any())
                    return Result.Failure(I18N
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.MealOfTheDayType));

                Repository.Delete(mu);
                await UnitOfWork.SaveAsync(cancellationToken);
                return Result.Success();
            });
    }
}
