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
    public class Product : Entity
    {
#warning Sparation Of Concerns violation
        //The default constructor is requred by Moq
        protected Product()
        {
        }

        private Product(Name name)
        {
            this.Name = name;
        }

        public Name Name { get; private set; }

        public static Result<Product> Create(string? name, IResources resources, IUnitOfWork unitOfWork)
        {
            var nameResult = Name.Create(name, resources, x => x.CommonTerms.Product, Constants.FIFTY);
            if (nameResult.IsFailure)
                return nameResult.ConvertFailure<Product>();

            var product = unitOfWork.ProductRepository.GetFirstOrDefault(x => x.Name == name);
            if (product != null)
                return Result.Failure<Product>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => product.Name));

            var entity = new Product(nameResult.Value);
            return Result.Success(entity);
        }

#warning Sparation Of Concerns violation
        //virtural keyword is required by Moq to be able to mock the method
        public virtual Result<Product> Update(string? name, IResources resources, IUnitOfWork unitOfWork)
        {
            var nameResult = Name.Create(name, resources, x => x.CommonTerms.Product, Constants.FIFTY);
            if (nameResult.IsFailure)
                return nameResult.ConvertFailure<Product>();

            var product = unitOfWork.ProductRepository.GetFirstOrDefault(x => x.Name == name);
            if (product != null)
                return Result.Failure<Product>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => product.Name));

            this.Name = nameResult.Value;
            return Result.Success(this);
        }
    }
}
