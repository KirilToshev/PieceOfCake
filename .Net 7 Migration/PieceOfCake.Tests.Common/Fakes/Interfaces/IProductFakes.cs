using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Tests.Common.Fakes.Interfaces;
public interface IProductFakes : INameFakes<Product>
{
    Product Pepper { get; }
    Product Water { get; }
    Product Carrot { get; }
}
