using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Tests.Common.Fakes.Common;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Tests.Common.Fakes;
public class ProductFakes : NameFakes<Product>, IProductFakes
{
    public ProductFakes (IResources resources, IUnitOfWork uowMock)
        : base(resources, uowMock)
    {
    }

    public Product Carrot => Create(TestsConstants.Products.CARROT);
    public Product Water => Create(TestsConstants.Products.WATER);
    public Product Pepper => Create(TestsConstants.Products.PEPPER);

    public override Func<string, IResources, IUnitOfWork, CancellationToken, Task<Result<Product>>> CreateFunction => Product.CreateAsync;

    public override Expression<Func<Product, string>> CacheKey => x => x.Name;
}
