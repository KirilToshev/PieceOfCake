using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;

//This is required to suppress warnings/errors in the default(empty) constructor
//required by Moq to construct this object in the UnitTests
#pragma warning disable 8618
namespace PieceOfCake.Core.Entities
{
    public class MeasureUnit : Common.Entity
    {
#pragma warning disable 1030
#warning Sparation Of Concerns violation
        //The default constructor is requred by Moq and EF Core
        protected MeasureUnit()
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

            

            var entity = new MeasureUnit(nameResult.Value);
            return Result.Success(entity);
        }

        #warning Sparation Of Concerns violation
        //virtural keyword is required by Moq to be able to mock the method
        public virtual Result<MeasureUnit> Update(string? name, IResources resources, IUnitOfWork unitOfWork)
        {
            var nameResult = Name.Create(name, resources, x => x.CommonTerms.MeasureUnit, Constants.FIFTY);
            if (nameResult.IsFailure)
                return nameResult.ConvertFailure<MeasureUnit>();

            var measureUnit = unitOfWork.MeasureUnitRepository.GetFirstOrDefault(x => x.Name == name);
            if (measureUnit != null)
                return Result.Failure<MeasureUnit>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => measureUnit.Name));

            this.Name = nameResult.Value;
            return Result.Success(this);
        }
    }
}
