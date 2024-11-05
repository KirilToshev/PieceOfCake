using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Application.DishFeature.Services;

public class MealOfTheDayTypeService : IMealOfTheDayTypeService
{
    private readonly IResources _resources;
    private readonly IUnitOfWork _unitOfWork;

    public MealOfTheDayTypeService (
        IResources resources,
        IUnitOfWork unitOfWork)
    {
        _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public IReadOnlyCollection<MealOfTheDayType> Get ()
        => _unitOfWork.MealOfTheDayTypeRepository.GetAsync();

    public Result<MealOfTheDayType> Get (Guid id)
    {
        var mealType = _unitOfWork.MealOfTheDayTypeRepository.GetById(id);

        if (mealType == null)
            return Result.Failure<MealOfTheDayType>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return Result.Success(mealType);
    }

    public Result<MealOfTheDayType> Update (Guid id, string name)
    {
        var mealType = _unitOfWork.MealOfTheDayTypeRepository.GetById(id);

        if (mealType == null)
            return Result.Failure<MealOfTheDayType>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return mealType.Update(name, _resources, _unitOfWork)
            .Tap(x =>
            {
                _unitOfWork.MealOfTheDayTypeRepository.Update(x);
                _unitOfWork.Save();
            });
    }
    public Result<MealOfTheDayType> Create (string name)
    {
        return MealOfTheDayType.Create(name, _resources, _unitOfWork)
            .Tap(x =>
            {
                _unitOfWork.MealOfTheDayTypeRepository.Insert(x);
                _unitOfWork.Save();
            });
    }

    public Result Delete (Guid id)
    {
        return Get(id)
            .Bind(mu =>
            {
                var isMealTypeInUse = _unitOfWork.DishRepository
                                        .GetAsync(dish => dish.MealOfTheDayTypes.Select(x => x.Id).Contains(id))
                                        .Any();

                if (!isMealTypeInUse)
                    isMealTypeInUse = _unitOfWork.MenuRepository
                        .GetAsync(menu => menu.MealOfTheDayTypes.Contains(mu)).Any();

                if (isMealTypeInUse)
                    return Result.Failure(_resources
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.MealOfTheDayType));

                _unitOfWork.MealOfTheDayTypeRepository.Delete(mu);
                _unitOfWork.Save();
                return Result.Success();
            });
    }
}
