using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Common.ValueObjects;

namespace PieceOfCake.Core.IngredientFeature.Entities;

public class Product : GuidEntity
{
    protected Product ()
    {

    }

    private Product (Name name)
    {
        Name = name;
    }

    public virtual Name Name { get; private set; }

    public static async Task<Result<Product>> CreateAsync (string name, IResources resources, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(name, resources, x => x.CommonTerms.Product, Constants.FIFTY);
        if (nameResult.IsFailure)
            return nameResult.ConvertFailure<Product>();

        var product = await unitOfWork.ProductRepository.FirstOrDefaultAsync(cancellationToken, x => x.Name == name);
        if (product != null)
            return Result.Failure<Product>(resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => product.Name));

        var entity = new Product(nameResult.Value);
        return Result.Success(entity);
    }

    //TODO: Methods are virtual only for NSubstitute to be able to mock them. Find a better solution!
    public virtual async Task<Result<Product>> UpdateAsync (string name, IResources resources, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var productResult = await CreateAsync(name, resources, unitOfWork, cancellationToken);
        if (productResult.IsFailure)
            return productResult.ConvertFailure<Product>();

        Name = productResult.Value.Name;
        return Result.Success(this);
    }
}
