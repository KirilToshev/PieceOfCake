using CSharpFunctionalExtensions;
using PieceOfCake.Core.Resources;
using System;
using System.Linq.Expressions;

namespace PieceOfCake.Core.ValueObjects
{
    public class Name : ValueObject<Name>
    {
        private readonly string _value;

        protected Name()
        {
            _value = string.Empty;
        }

        private Name(string name)
        {
            _value = name;
        }

        public static implicit operator string(Name name) => name._value;

        public static Result<Name> Create(string? name, IResources resources, Expression<Func<IResources, string?>> entityName, uint maxLength, uint? minLength = null)
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

        protected override bool EqualsCore(Name other)
        {
            return _value.Equals(other._value);
        }

        protected override int GetHashCodeCore()
        {
            return _value.GetHashCode() ^ 583;
        }
    }
}
