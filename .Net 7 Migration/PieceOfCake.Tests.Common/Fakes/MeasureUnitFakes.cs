using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Tests.Common.Fakes;
public class MeasureUnitFakes : NameFakes<MeasureUnit>
{
    public MeasureUnitFakes (IResources resources, IUnitOfWork uowMock)
        : base(resources, uowMock, MeasureUnit.Create)
    {
    }

    public MeasureUnit Litter => Create(TestsConstants.MeasureUnits.LITTER);
    public MeasureUnit Kg => Create(TestsConstants.MeasureUnits.KG);
    public MeasureUnit Number => Create(TestsConstants.MeasureUnits.NUMBER);
}
