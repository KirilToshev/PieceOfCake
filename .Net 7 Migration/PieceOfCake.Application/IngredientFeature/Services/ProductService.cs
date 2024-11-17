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

    public async Task<IReadOnlyCollection<ProductGetDto>> GetAllAsync ()
    {
        var products = await Repository.GetAsync();
        return products.Select(p => p.MapToGetDto()).ToArray().AsReadOnly();
    }

    public Task<Result<ProductGetDto>> GetByIdAsync (Guid id)
    {
        return GetEntityAsync(id).Map(x => x.MapToGetDto());
    }

    public async Task<Result<ProductGetDto>> UpdateAsync (ProductUpdateDto updateDto)
    {
        return await GetEntityAsync(updateDto.Id)
            .Bind(product => product.Update(product.Name, I18N, UnitOfWork)
            .Map(async product =>
            {
                Repository.Update(product);
                await SaveAsync();
                return product.MapToGetDto();
            }));
    }

    public Task<Result<ProductGetDto>> CreateAsync (ProductCreateDto createDto)
    {
        return Product.Create(createDto.Name, I18N, UnitOfWork)
            .Map(async product =>
            {
                Repository.Insert(product);
                await SaveAsync();
                return product.MapToGetDto();
            });
    }

    public async Task<Result> DeleteAsync (Guid id)
    {
        return await GetEntityAsync(id)
            .Bind(async product =>
            {
                var products = await UnitOfWork.DishRepository.GetAsync(dish => dish.Ingredients.Any(i => i.Product.Id == product.Id));
                var isProductInUse = products.Any();

                if (isProductInUse)
                    return Result.Failure(I18N
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Product));

                Repository.Delete(product);
                await SaveAsync();
                return Result.Success();
            });
    }
}
