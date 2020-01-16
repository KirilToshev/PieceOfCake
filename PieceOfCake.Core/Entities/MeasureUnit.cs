using CSharpFunctionalExtensions;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace PieceOfCake.Core.Entities
{
    public class MeasureUnit : ValueObject<MeasureUnit>
    {
        private const int NAME_MAX_LENGHT = 50;

        private MeasureUnit(string name)
        {
            this.Name = name;
        }

        [Key]
        public int Id { get; protected set; }

        [MaxLength(NAME_MAX_LENGHT)]
        public string Name { get; private set; }

        public static Result<MeasureUnit>Create(string? name, IResources resources, IUnitOfWork unitOfWork)
        {
            var nameResult = ValueObjects.Name.Create(name, resources, x => x.CommonTerms.MeasureUnit, NAME_MAX_LENGHT);
            if (nameResult.IsFailure)
                return nameResult.ConvertFailure<MeasureUnit>();

            var measureUnit = unitOfWork.MeasureUnitRepository.GetFirstOrDefault(x => x.Name == name);
            if (measureUnit != null)
                return Result.Failure<MeasureUnit>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => measureUnit.Name));

            return Result.Ok(new MeasureUnit(nameResult.Value));
        }

        public Result<MeasureUnit> Update(string? name, IResources resources, IUnitOfWork unitOfWork)
        {
            var nameResult = ValueObjects.Name.Create(name, resources, x => x.CommonTerms.MeasureUnit, NAME_MAX_LENGHT);
            if (nameResult.IsFailure)
                return nameResult.ConvertFailure<MeasureUnit>();

            var measureUnit = unitOfWork.MeasureUnitRepository.GetFirstOrDefault(x => x.Name == name);
            if (measureUnit != null)
                return Result.Failure<MeasureUnit>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => measureUnit.Name));

            this.Name = nameResult.Value;

            return Result.Ok(this);
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
