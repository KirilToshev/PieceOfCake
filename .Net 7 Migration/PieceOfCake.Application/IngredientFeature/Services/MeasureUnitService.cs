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
    private readonly IResources _resources;
    private readonly IUnitOfWork _unitOfWork;

    protected override IMeasureUnitRepository Repository => _unitOfWork.MeasureUnitRepository;

    public MeasureUnitService (
        IResources resources,
        IUnitOfWork unitOfWork)
        : base(resources)
    {
        _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public IReadOnlyCollection<MeasureUnitGetDto> GetAllAsync () => 
        Repository.GetAsync()
        .Select(x => x.MapToGetDto())
        .ToArray().AsReadOnly();

    public Result<MeasureUnitGetDto> GetByIdAsync (Guid id)
    {
        return GetEntity(id).Map(x => x.MapToGetDto());
    }

    public Result<MeasureUnitGetDto> UpdateAsync (MeasureUnitUpdateDto updateDto)
    {
        var measureUnit = Repository.GetById(updateDto.Id);

        if (measureUnit == null)
            return Result.Failure<MeasureUnitGetDto>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => updateDto.Id.ToString()));

        return GetEntity(updateDto.Id)
            .Bind(measureUnit =>  measureUnit.Update(updateDto.Name, _resources, _unitOfWork)
            .Map(measureUnit =>
            {
                Repository.Update(measureUnit);
                _unitOfWork.Save();
                return measureUnit.MapToGetDto();
            }));
    }

    public Result<MeasureUnitGetDto> CreateAsync (MeasureUnitCreateDto createDto)
    {
        return MeasureUnit.Create(createDto.Name, _resources, _unitOfWork)
            .Map(x =>
            {
                Repository.Insert(x);
                _unitOfWork.Save();
                return x.MapToGetDto();
            });
    }

    public async Task<Result> DeleteAsync (Guid id)
    {
        return await GetEntity(id)
            .Bind(async mu =>
            {
                var isMeasureUnitInUse = await _unitOfWork.DishRepository
                                        .GetAsync(dish => dish.Ingredients.Any(i => i.MeasureUnit.Id == mu.Id));
                if (isMeasureUnitInUse.Any())
                    return Result.Failure(_resources
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.MeasureUnit));

                Repository.Delete(mu);
                _unitOfWork.Save();
                return Result.Success();
            });
    }
}
