using CSharpFunctionalExtensions;
using PieceOfCake.Core.Resources;
using System;
using System.Linq.Expressions;

namespace PieceOfCake.Core.ValueObjects
{
    public class Name
    {
        private readonly string _value;

        private Name(string name)
        {
            _value = name;
        }

        public static implicit operator string(Name name) => name._value;

        public static Result<Name> Create(string? name, IResources resources, Expression<Func<IResources, string>> entityName, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Name>(resources.GenereteSentence(x => x.UserErrors.NameIsMandatory, entityName));

            if (name.Length > maxLength)
                return Result.Failure<Name>(resources.GenereteSentence(x => x.UserErrors.NameExceedsMaxLength, entityName, x => maxLength.ToString()));

            return Result.Success(new Name(name));
        }
    }
}
