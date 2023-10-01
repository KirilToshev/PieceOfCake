using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Tests.Common.Fakes;
public class ProductFakes : NameFakes<Product>
{
    public ProductFakes (IResources resources, IUnitOfWork uowMock) 
        : base(resources, uowMock, Product.Create)
    {
    }

    public Product Carrot => Create(TestsConstants.Products.CARROT);
    public Product Water => Create(TestsConstants.Products.WATER);
    public Product Pepper => Create(TestsConstants.Products.PEPPER);
}
