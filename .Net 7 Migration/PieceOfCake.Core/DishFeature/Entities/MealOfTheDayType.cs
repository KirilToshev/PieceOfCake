using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Common.ValueObjects;

namespace PieceOfCake.Core.DishFeature.Entities;
public class MealOfTheDayType : GuidEntity
{
    protected MealOfTheDayType ()
    {

    }

    private MealOfTheDayType (Name name)
    {
        Name = name;
    }

    public virtual Name Name { get; private set; }

    public static Result<MealOfTheDayType> Create (string? name, IResources resources, IUnitOfWork unitOfWork)
    {
        var nameResult = Name.Create(name, resources, x => x.CommonTerms.MealOfTheDayType, Constants.FIFTY);
        if (nameResult.IsFailure)
            return nameResult.ConvertFailure<MealOfTheDayType>();

        var mealOfTheDayType = unitOfWork.MealOfTheDayTypeRepository.GetFirstOrDefault(x => x.Name == name);
        if (mealOfTheDayType != null)
            return Result.Failure<MealOfTheDayType>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => mealOfTheDayType.Name));

        var entity = new MealOfTheDayType(nameResult.Value);
        return Result.Success(entity);
    }

    public virtual Result<MealOfTheDayType> Update (string? name, IResources resources, IUnitOfWork unitOfWork)
    {
        var mealOfTheDayTypeResult = Create(name, resources, unitOfWork);
        if (mealOfTheDayTypeResult.IsFailure)
            return mealOfTheDayTypeResult.ConvertFailure<MealOfTheDayType>();

        Name = mealOfTheDayTypeResult.Value.Name;
        return Result.Success(this);
    }
}
