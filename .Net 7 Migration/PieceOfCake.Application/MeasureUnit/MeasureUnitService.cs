using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Application.MeasureUnit;

public class MeasureUnitService : IMeasureUnitService
{
    private readonly IResources _resources;
    private readonly IUnitOfWork _unitOfWork;

    public MeasureUnitService (
        IResources resources,
        IUnitOfWork unitOfWork)
    {
        _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public IReadOnlyCollection<Core.MeasureUnit.MeasureUnit> Get () => _unitOfWork.MeasureUnitRepository.Get();

    public Result<Core.MeasureUnit.MeasureUnit> Get (Guid id)
    {
        var measureUnit = _unitOfWork.MeasureUnitRepository.GetById(id);

        if (measureUnit == null)
            return Result.Failure<Core.MeasureUnit.MeasureUnit>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return Result.Success(measureUnit);
    }

    public Result<Core.MeasureUnit.MeasureUnit> Update (Guid id, string? name)
    {
        var measureUnit = _unitOfWork.MeasureUnitRepository.GetById(id);

        if (measureUnit == null)
            return Result.Failure<Core.MeasureUnit.MeasureUnit>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return measureUnit.Update(name, _resources, _unitOfWork)
            .Tap(x =>
            {
                _unitOfWork.MeasureUnitRepository.Update(x);
                _unitOfWork.Save();
            });
    }

    public Result<Core.MeasureUnit.MeasureUnit> Create (string name)
    {
        return Core.MeasureUnit.MeasureUnit.Create(name, _resources, _unitOfWork)
            .Tap(x =>
            {
                _unitOfWork.MeasureUnitRepository.Insert(x);
                _unitOfWork.Save();
            });
    }

    public Result Delete (Guid id)
    {
        return Get(id)
            .Bind(mu =>
            {
                var isMeasureUnitInUse = _unitOfWork.DishRepository
                                        .Get(dish => dish.Ingredients.Any(i => i.MeasureUnit.Id == mu.Id))
                                        .Any();
                if (isMeasureUnitInUse)
                    return Result.Failure(_resources
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.MeasureUnit));

                _unitOfWork.MeasureUnitRepository.Delete(mu);
                _unitOfWork.Save();
                return Result.Success();
            });
    }
}