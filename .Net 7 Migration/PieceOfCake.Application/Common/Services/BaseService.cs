using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Application.Common.Services;

public abstract class BaseService<IRepository, TEntity>
    where IRepository : IGenericRepository<TEntity>
    where TEntity : class
{
    private readonly IResources _resources;

    public BaseService (IResources resources)
    {
        _resources = resources;
    }

    protected abstract IRepository Repository { get; }

    protected Result<TEntity> GetEntity (Guid id)
    {
        var measureUnit = Repository.GetById(id);

        if (measureUnit == null)
            return Result.Failure<TEntity>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return Result.Success(measureUnit);
    }
}
