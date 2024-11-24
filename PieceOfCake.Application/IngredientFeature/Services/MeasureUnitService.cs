using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Application.IngredientFeature.Dtos.Mapping;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Application.IngredientFeature.Services;

public class MeasureUnitService : BaseService<IMeasureUnitRepository, MeasureUnit>, IMeasureUnitService
{
    protected override IMeasureUnitRepository Repository => UnitOfWork.MeasureUnitRepository;

    public MeasureUnitService (
        IResources resources,
        IUnitOfWork unitOfWork)
        : base(resources, unitOfWork)
    {
    }

    public async Task<IReadOnlyCollection<MeasureUnitGetDto>> GetAllAsync (CancellationToken cancellationToken)
    {
        var mesureUnits = await Repository.GetAsync(cancellationToken);
        return mesureUnits.Select(x => x.MapToGetDto()).ToArray().AsReadOnly();
    }
        

    public Task<Result<MeasureUnitGetDto>> GetByIdAsync (Guid id, CancellationToken cancellationToken)
    {
        return GetEntityAsync(id, cancellationToken).Map(x => x.MapToGetDto());
    }

    public Task<Result<MeasureUnitGetDto>> UpdateAsync (MeasureUnitUpdateDto updateDto, CancellationToken cancellationToken)
    {
        return GetEntityAsync(updateDto.Id, cancellationToken)
            .Bind(measureUnit =>  measureUnit.UpdateAsync(updateDto.Name, I18N, UnitOfWork, cancellationToken)
            .Map(async measureUnit =>
            {
                Repository.Update(measureUnit);
                await SaveAsync(cancellationToken);
                return measureUnit.MapToGetDto();
            }));
    }

    public Task<Result<MeasureUnitGetDto>> CreateAsync (MealOfTheDayTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        return MeasureUnit.CreateAsync(createDto.Name, I18N, UnitOfWork, cancellationToken)
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
                                        .GetAsync(cancellationToken, 
                                        dish => dish.Ingredients
                                        .Any(i => i.MeasureUnit.Id == mu.Id));
                if (usedInDishesList.Any())
                    return Result.Failure(I18N.GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.MeasureUnit));

                Repository.Delete(mu);
                await SaveAsync(cancellationToken);
                return Result.Success();
            });
    }
}
