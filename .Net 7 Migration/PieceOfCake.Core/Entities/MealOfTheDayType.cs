using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;

namespace PieceOfCake.Core.Entities;
public class MealOfTheDayType : Entity<Guid>
{
    protected MealOfTheDayType ()
    {

    }

    private MealOfTheDayType (Name name)
    {
        this.Name = name;
    }

    public virtual Name Name { get; private set; }

    public static Result<MealOfTheDayType> Create (string? name, IResources resources, IUnitOfWork unitOfWork)
    {
        var nameResult = Name.Create(name, resources, x => x.CommonTerms.MealOfTheDayType, Constants.FIFTY);
        if (nameResult.IsFailure)
            return nameResult.ConvertFailure<MealOfTheDayType>();

        var product = unitOfWork.MealOfTheDayTypeRepository.GetFirstOrDefault(x => x.Name == name);
        if (product != null)
            return Result.Failure<MealOfTheDayType>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => product.Name));

        var entity = new MealOfTheDayType(nameResult.Value);
        return Result.Success(entity);
    }

    public virtual Result<MealOfTheDayType> Update (string? name, IResources resources, IUnitOfWork unitOfWork)
    {
        var mealOfTheDayTypeResult = Create(name, resources, unitOfWork);
        if (mealOfTheDayTypeResult.IsFailure)
            return mealOfTheDayTypeResult.ConvertFailure<MealOfTheDayType>();

        this.Name = mealOfTheDayTypeResult.Value.Name;
        return Result.Success(this);
    }
}
