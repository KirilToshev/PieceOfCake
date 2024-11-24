using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Tests.Common.Fakes.Common;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Tests.Common.Fakes;
public class MeasureUnitFakes : NameFakes<MeasureUnit>, IMeasureUnitFakes
{
    public MeasureUnitFakes (IResources resources, IUnitOfWork uowMock)
        : base(resources, uowMock)
    {
    }

    public MeasureUnit Litter => Create(TestsConstants.MeasureUnits.LITTER);
    public MeasureUnit Kg => Create(TestsConstants.MeasureUnits.KG);
    public MeasureUnit Number => Create(TestsConstants.MeasureUnits.NUMBER);

    public override Func<string, IResources, IUnitOfWork, CancellationToken, Task<Result<MeasureUnit>>> CreateFunction => MeasureUnit.CreateAsync;

    public override Expression<Func<MeasureUnit, string>> CacheKey => x => x.Name;
}
