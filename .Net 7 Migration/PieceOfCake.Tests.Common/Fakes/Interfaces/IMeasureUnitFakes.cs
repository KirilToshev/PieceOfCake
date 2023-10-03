using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Tests.Common.Fakes.Interfaces;
public interface IMeasureUnitFakes : INameFakes<MeasureUnit>
{
    MeasureUnit Kg { get; }
    MeasureUnit Litter { get; }
    MeasureUnit Number { get; }
}
