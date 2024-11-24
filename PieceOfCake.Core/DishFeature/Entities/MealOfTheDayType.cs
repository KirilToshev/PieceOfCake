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

    public static async Task<Result<MealOfTheDayType>> Create (string? name, IResources resources, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(name, resources, x => x.CommonTerms.MealOfTheDayType, Constants.FIFTY);
        if (nameResult.IsFailure)
            return nameResult.ConvertFailure<MealOfTheDayType>();

        var mealOfTheDayType = await unitOfWork.MealOfTheDayTypeRepository.FirstOrDefaultAsync(cancellationToken, x => x.Name == name);
        if (mealOfTheDayType != null)
            return Result.Failure<MealOfTheDayType>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => mealOfTheDayType.Name));

        var entity = new MealOfTheDayType(nameResult.Value);
        return entity;
    }

    public virtual async Task<Result<MealOfTheDayType>> UpdateAsync (string? name, IResources resources, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var mealOfTheDayTypeResult = await Create(name, resources, unitOfWork, cancellationToken);
        if (mealOfTheDayTypeResult.IsFailure)
            return mealOfTheDayTypeResult.ConvertFailure<MealOfTheDayType>();

        Name = mealOfTheDayTypeResult.Value.Name;
        return this;
    }
}
