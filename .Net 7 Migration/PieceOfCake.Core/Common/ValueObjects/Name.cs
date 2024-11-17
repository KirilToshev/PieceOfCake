using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Resources;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Common.ValueObjects;

public class Name : ValueObject<Name, string>
{
    protected Name ()
        : base(string.Empty)
    {
    }

    protected Name (string name)
        : base(name)
    {
    }

    public static implicit operator string (Name name) => name.Value;

    public static Result<Name> Create (string? name, IResources resources, Expression<Func<IResources, string>> entityName, uint maxLength, uint? minLength = null)
    {
        if (maxLength == 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength));
        if (minLength.HasValue && maxLength < minLength)
            throw new ArgumentOutOfRangeException($"{nameof(maxLength)} must be grater than {nameof(minLength)}");

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Name>(resources.GenereteSentence(x => x.UserErrors.NameIsMandatory, entityName));

        if (name.Length > maxLength)
            return Result.Failure<Name>(resources.GenereteSentence(x => x.UserErrors.NameExceedsMaxLength, entityName, x => maxLength.ToString()));

        if (minLength.HasValue && name.Length < minLength)
            return Result.Failure<Name>(resources.GenereteSentence(x => x.UserErrors.NameBelowMinLength, entityName, x => minLength.Value.ToString()));

        return Result.Success(new Name(name));
    }
}
