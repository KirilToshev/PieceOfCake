using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Application.Product.Services;

public class ProductService : IProductService
{
    private readonly IResources _resources;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService (
        IResources resources,
        IUnitOfWork unitOfWork)
    {
        _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public IReadOnlyCollection<Core.Entities.Product> Get () => _unitOfWork.ProductRepository.Get();

    public Result<Core.Entities.Product> Get (Guid id)
    {
        var product = _unitOfWork.ProductRepository.GetById(id);

        if (product == null)
            return Result.Failure<Core.Entities.Product>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return Result.Success(product);
    }

    public Result<Core.Entities.Product> Update (Guid id, string? name)
    {
        var product = _unitOfWork.ProductRepository.GetById(id);

        if (product == null)
            return Result.Failure<Core.Entities.Product>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return product.Update(name, _resources, _unitOfWork)
            .Tap(x =>
            {
                _unitOfWork.ProductRepository.Update(x);
                _unitOfWork.Save();
            });
    }

    public Result<Core.Entities.Product> Create (string name)
    {
        return Core.Entities.Product.Create(name, _resources, _unitOfWork)
            .Tap(x =>
            {
                _unitOfWork.ProductRepository.Insert(x);
                _unitOfWork.Save();
            });
    }

    public Result Delete (Guid id)
    {
        return Get(id)
            .Bind(product =>
            {
                var isProductInUse = _unitOfWork.DishRepository
                                        .Get(dish => dish.Ingredients.Any(i => i.Product.Id == product.Id))
                                        .Any();
                if (isProductInUse)
                    return Result.Failure(_resources
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Product));

                _unitOfWork.ProductRepository.Delete(product);
                _unitOfWork.Save();
                return Result.Success();
            });
    }
}
