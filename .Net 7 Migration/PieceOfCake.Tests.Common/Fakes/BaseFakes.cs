using AutoFixture;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Tests.Common.Fakes;
public class BaseFakes
{
    protected readonly Fixture _fixture;
    protected readonly IResources _resources;
    protected readonly IUnitOfWork _uowMock;

    public BaseFakes (
        IResources resources,
        IUnitOfWork uowMock)
    {
        _resources = resources;
        _uowMock = uowMock;
        _fixture = new Fixture();
    }
}
