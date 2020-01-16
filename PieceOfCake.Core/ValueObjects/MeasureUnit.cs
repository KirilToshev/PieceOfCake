using CSharpFunctionalExtensions;
using Microsoft.Extensions.Localization;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace PieceOfCake.Core.ValueObjects
{
    public class MeasureUnit : ValueObject<MeasureUnit>
    {
        private MeasureUnit(string name)
        {
            this.Name = name;
        }

        [Key]
        public int Id { get; protected set; }

        [MaxLength(50)]
        public string Name { get; private set; }

        public static Result<MeasureUnit>Create(string? name, IResources resources)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<MeasureUnit>(resources.GenereteSentence(x => x.UserErrors.NameIsMandatory, x => x.CommonTerms.MeasureUnit));

            if (name.Length > 50)
                return Result.Failure<MeasureUnit>(resources.GenereteSentence(x => x.UserErrors.NameExceedsMaxLength, x => x.CommonTerms.MeasureUnit, x => "50"));

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
