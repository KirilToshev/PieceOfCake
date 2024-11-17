using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Application.Common.Services;

public abstract class BaseService<IRepository, TEntity> : IDisposable
    where IRepository : IGenericRepository<TEntity>
    where TEntity : class
{
    public BaseService (IResources i18n, IUnitOfWork unitOfWork)
    {
        I18N = i18n ?? throw new ArgumentNullException(nameof(i18n));
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    protected IResources I18N { get; }

    protected IUnitOfWork UnitOfWork { get; }

    protected abstract IRepository Repository { get; }

    protected async Task<Result<TEntity>> GetEntityAsync (Guid id)
    {
        var entity = await Repository.GetByIdAsync(id);

        if (entity == null)
            return Result.Failure<TEntity>(
                I18N.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return Result.Success(entity);
    }

    public Task<int> SaveAsync()
    {
        return UnitOfWork.SaveAsync();
    }

    public void Dispose ()
    {
        UnitOfWork.Dispose();
    }
}
