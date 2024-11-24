using AutoFixture;
using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Tests.Common.Fakes.Interfaces;

namespace PieceOfCake.Tests.Common.Fakes.Common;

public abstract class NameFakes<TValue> : EntityFakes<string, TValue>, INameFakes<TValue> where TValue : GuidEntity
{
    public NameFakes (
        IResources resources,
        IUnitOfWork uowMock
        )
        : base(resources, uowMock)
    {
    }

    public abstract Func<string, IResources, IUnitOfWork, CancellationToken, Task<Result<TValue>>> CreateFunction { get; }

    public TValue Create (string? name = null)
    {
        if (name is null)
            name = _fixture.Create<string>();
        var createResult = Task.Run(async () => await CreateFunction(name, _resources, _uowMock, CancellationToken.None));
        createResult.Wait();
        return GetFromCache(createResult.Result.Value);
    }
}
