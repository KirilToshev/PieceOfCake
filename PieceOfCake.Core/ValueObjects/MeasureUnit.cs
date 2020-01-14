using CSharpFunctionalExtensions;
using Microsoft.Extensions.Localization;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Core.ValueObjects
{
    public class MeasureUnit : ValueObject<MeasureUnit>
    {
        private MeasureUnit(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public static Result<MeasureUnit>Create(string? name, IUserErrorsResource userErrors)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<MeasureUnit>(string.Format(userErrors.NameIsMandatory, "Measure Unit"));
             
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
