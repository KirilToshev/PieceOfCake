using AutoFixture;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Tests.Common.Fakes.Common;
public abstract class BaseFakes
{
    protected readonly Fixture _fixture;
    protected readonly IResources _resources;
    protected readonly IUnitOfWork _uowMock;

    public BaseFakes (
        IResources resources,
        IUnitOfWork uowMock)
    {
        _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        _uowMock = uowMock ?? throw new ArgumentNullException(nameof(uowMock));
        _fixture = new Fixture();
    }
}
