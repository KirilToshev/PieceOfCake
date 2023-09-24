using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.ValueObjects;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Core.Product;

public class Product : Entity<Guid>
{
    protected Product ()
    {

    }

    private Product (Name name)
    {
        Name = name;
    }

    public virtual Name Name { get; private set; }

    public static Result<Product> Create (string name, IResources resources, IUnitOfWork unitOfWork)
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

    public virtual Result<Product> Update (string name, IResources resources, IUnitOfWork unitOfWork)
    {
        var productResult = Create(name, resources, unitOfWork);
        if (productResult.IsFailure)
            return productResult.ConvertFailure<Product>();

        Name = productResult.Value.Name;
        return Result.Success(this);
    }
}
