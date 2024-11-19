using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Common.ValueObjects;

namespace PieceOfCake.Core.IngredientFeature.Entities;

public class MeasureUnit : GuidEntity
{
    protected MeasureUnit ()
    {

    }

    private MeasureUnit (Name name)
    {
        Name = name;
    }

    public Name Name { get; private set; }

    public static async Task<Result<MeasureUnit>> CreateAsync (string? name, IResources i18n, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(name, i18n, x => x.CommonTerms.MeasureUnit, Constants.FIFTY);
        if (nameResult.IsFailure)
            return nameResult.ConvertFailure<MeasureUnit>();

        var measureUnit = await unitOfWork.MeasureUnitRepository.FirstOrDefaultAsync(cancellationToken, x => x.Name == name);
        if (measureUnit != null)
            return Result.Failure<MeasureUnit>(i18n.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => measureUnit.Name));

        var entity = new MeasureUnit(nameResult.Value);
        return Result.Success(entity);
    }

    public virtual async Task<Result<MeasureUnit>> UpdateAsync (string? name, IResources resources, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var measureUnitResult = await CreateAsync(name, resources, unitOfWork, cancellationToken);
        if (measureUnitResult.IsFailure)
            return measureUnitResult.ConvertFailure<MeasureUnit>();

        Name = measureUnitResult.Value.Name;
        return Result.Success(this);
    }
}
