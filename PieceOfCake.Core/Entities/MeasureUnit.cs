using CSharpFunctionalExtensions;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

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

        public static Result<MeasureUnit> Create(string? name, IResources resources, IUnitOfWork unitOfWork)
        {
            return CommonNameValidation(name, resources, unitOfWork,
                validName =>
                {
                    var entity = new MeasureUnit(validName);
                    unitOfWork.MeasureUnitRepository.Insert(entity);
                    return Result.Success(entity);
                });
        }

        public Result<MeasureUnit> Update(string? name, IResources resources, IUnitOfWork unitOfWork)
        {
            return CommonNameValidation(name, resources, unitOfWork,
                validName =>
                {
                    this.Name = validName;
                    unitOfWork.MeasureUnitRepository.Update(this);
                    return Result.Success(this);
                });
        }

        private static Result<MeasureUnit> CommonNameValidation(string? name, IResources resources, IUnitOfWork unitOfWork, Func<string, Result<MeasureUnit>> returnStatement)
        {
            var nameResult = ValueObjects.Name.Create(name, resources, x => x.CommonTerms.MeasureUnit, NAME_MAX_LENGHT);
            if (nameResult.IsFailure)
                return nameResult.ConvertFailure<MeasureUnit>();

            var measureUnit = unitOfWork.MeasureUnitRepository.GetFirstOrDefault(x => x.Name == name);
            if (measureUnit != null)
                return Result.Failure<MeasureUnit>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => measureUnit.Name));

            return returnStatement.Invoke(nameResult.Value);
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
