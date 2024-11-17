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

    public async Task<IReadOnlyCollection<MeasureUnitGetDto>> GetAllAsync ()
    {
        var mesureUnits = await Repository.GetAsync();
        return mesureUnits.Select(x => x.MapToGetDto()).ToArray().AsReadOnly();
    }
        

    public Task<Result<MeasureUnitGetDto>> GetByIdAsync (Guid id)
    {
        return GetEntityAsync(id).Map(x => x.MapToGetDto());
    }

    public async Task<Result<MeasureUnitGetDto>> UpdateAsync (MeasureUnitUpdateDto updateDto)
    {
        return await GetEntityAsync(updateDto.Id)
            .Bind(measureUnit =>  measureUnit.Update(updateDto.Name, I18N, UnitOfWork)
            .Map(async measureUnit =>
            {
                Repository.Update(measureUnit);
                await SaveAsync();
                return measureUnit.MapToGetDto();
            }));
    }

    public Task<Result<MeasureUnitGetDto>> CreateAsync (MeasureUnitCreateDto createDto)
    {
        return MeasureUnit.Create(createDto.Name, I18N, UnitOfWork)
            .Map(async x =>
            {
                Repository.Insert(x);
                await SaveAsync();
                return x.MapToGetDto();
            });
    }

    public async Task<Result> DeleteAsync (Guid id)
    {
        return await GetEntityAsync(id)
            .Bind(async mu =>
            {
                //TODO: Implement specification pattern
                var isMeasureUnitInUse = await UnitOfWork.DishRepository
                                        .GetAsync(dish => dish.Ingredients.Any(i => i.MeasureUnit.Id == mu.Id));
                if (isMeasureUnitInUse.Any())
                    return Result.Failure(I18N.GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.MeasureUnit));

                Repository.Delete(mu);
                await SaveAsync();
                return Result.Success();
            });
    }
}
