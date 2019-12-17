using CSharpFunctionalExtensions;
using System;

namespace PieceOfCake.Core.ValueObjects
{
    public class MeasureUnit : ValueObject<MeasureUnit>
    {
        private MeasureUnit(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public static Result<MeasureUnit>Create(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<MeasureUnit>("Error");
             
            return Result.Ok(new MeasureUnit(name));
        }

        protected override bool EqualsCore(MeasureUnit other)
        {
            return this.Name == other.Name;
        }

        protected override int GetHashCodeCore()
        {
            return this.Name.GetHashCode() ^ 617;
        }
    }
}
