using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Application.IngredientFeature.Dtos.Mapping;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Application.IngredientFeature.Services;

public class ProductService : BaseService<IProductRepository, Product>, IProductService
{
    protected override IProductRepository Repository => UnitOfWork.ProductRepository;

    public ProductService (
        IResources resources,
        IUnitOfWork unitOfWork)
        : base (resources, unitOfWork)
    {
    }

    public async Task<IReadOnlyCollection<ProductGetDto>> GetAllAsync (CancellationToken cancellationToken)
    {
        var products = await Repository.GetAsync(cancellationToken);
        return products.Select(p => p.MapToGetDto()).ToArray().AsReadOnly();
    }

    public Task<Result<ProductGetDto>> GetByIdAsync (Guid id, CancellationToken cancellationToken)
    {
        return GetEntityAsync(id, cancellationToken).Map(x => x.MapToGetDto());
    }

    public async Task<Result<ProductGetDto>> UpdateAsync (ProductUpdateDto updateDto, CancellationToken cancellationToken)
    {
        return await GetEntityAsync(updateDto.Id, cancellationToken)
            .Bind(product => product.UpdateAsync(updateDto.Name, I18N, UnitOfWork, cancellationToken)
            .Map(async product =>
            {
                Repository.Update(product);
                await SaveAsync(cancellationToken);
                return product.MapToGetDto();
            }));
    }

    public Task<Result<ProductGetDto>> CreateAsync (ProductCreateDto createDto, CancellationToken cancellationToken)
    {
        return Product.CreateAsync(createDto.Name, I18N, UnitOfWork, cancellationToken)
            .Map(async product =>
            {
                Repository.Insert(product);
                await SaveAsync(cancellationToken);
                return product.MapToGetDto();
            });
    }

    public async Task<Result> DeleteAsync (Guid id, CancellationToken cancellationToken)
    {
        return await GetEntityAsync(id, cancellationToken)
            .Bind(async product =>
            {
                var products = await UnitOfWork.DishRepository.GetAsync(cancellationToken,
                    dish => dish.Ingredients.Any(i => i.Product.Id == product.Id));
                var isProductInUse = products.Any();

                if (isProductInUse)
                    return Result.Failure(I18N
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Product));

                Repository.Delete(product);
                await SaveAsync(cancellationToken);
                return Result.Success();
            });
    }
}
