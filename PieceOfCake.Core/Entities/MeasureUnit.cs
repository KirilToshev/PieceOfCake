using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Entities
{
    public class MeasureUnit : Entity
    {
        private MeasureUnit(Name name)
        {
            this.Name = name;
        }

        public Name Name { get; private set; }

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
                    return Result.Success(this);
                });
        }

        private static Result<MeasureUnit> CommonNameValidation(string? name, IResources resources, IUnitOfWork unitOfWork, Func<Name, Result<MeasureUnit>> returnCallback)
        {
            var nameResult = Name.Create(name, resources, x => x.CommonTerms.MeasureUnit, Constants.NAME_MAX_LENGHT);
            if (nameResult.IsFailure)
                return nameResult.ConvertFailure<MeasureUnit>();

            var measureUnit = unitOfWork.MeasureUnitRepository.GetFirstOrDefault(x => x.Name == name);
            if (measureUnit != null)
                return Result.Failure<MeasureUnit>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => measureUnit.Name));

            return returnCallback.Invoke(nameResult.Value);
        }
    }
}
