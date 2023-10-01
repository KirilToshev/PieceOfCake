using AutoFixture;
using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Tests.Common.Fakes;

public class NameFakes<T> : BaseFakes
{
    private readonly Func<string, IResources, IUnitOfWork, Result<T>> _createFunction;

    public NameFakes (
        IResources resources,
        IUnitOfWork uowMock,
        Func<string, IResources, IUnitOfWork, Result<T>> createFunction)
        : base(resources, uowMock)
    {
        this._createFunction = createFunction;
    }

    public T Create (string? name = null)
    {
        if (name is null)
            name = _fixture.Create<string>();
        return _createFunction(name, _resources, _uowMock).Value;
    }
}
