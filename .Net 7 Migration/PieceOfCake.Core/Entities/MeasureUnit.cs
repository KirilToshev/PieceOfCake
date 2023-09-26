using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;

namespace PieceOfCake.Core.Entities;

public class MeasureUnit : Entity<Guid>
{
    protected MeasureUnit ()
    {
            
    }

    private MeasureUnit(Name name)
    {
        this.Name = name;
    }

    public Name Name { get; private set; }

    public static Result<MeasureUnit> Create(string? name, IResources resources, IUnitOfWork unitOfWork)
    {
        var nameResult = Name.Create(name, resources, x => x.CommonTerms.MeasureUnit, Constants.FIFTY);
        if (nameResult.IsFailure)
            return nameResult.ConvertFailure<MeasureUnit>();

        var measureUnit = unitOfWork.MeasureUnitRepository.GetFirstOrDefault(x => x.Name == name);
        if (measureUnit != null)
            return Result.Failure<MeasureUnit>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => measureUnit.Name));

        var entity = new MeasureUnit(nameResult.Value);
        return Result.Success(entity);
    }

    public virtual Result<MeasureUnit> Update(string? name, IResources resources, IUnitOfWork unitOfWork)
    {
        var measureUnitResult = Create(name, resources, unitOfWork);
        if (measureUnitResult.IsFailure)
            return measureUnitResult.ConvertFailure<MeasureUnit>();

        this.Name = measureUnitResult.Value.Name;
        return Result.Success(this);
    }
}
