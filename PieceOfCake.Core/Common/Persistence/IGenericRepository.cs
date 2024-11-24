using System.Linq.Expressions;

namespace PieceOfCake.Core.Common.Persistence;

public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Get all entities from db
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<TEntity>> GetAsync (
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Get single entity by primary key
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync (object id, CancellationToken cancellationToken);

    /// <summary>
    /// Get first or default entity by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    Task<TEntity?> FirstOrDefaultAsync (
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Insert entity to db
    /// </summary>
    /// <param name="entity"></param>
    void Insert (TEntity entity);

    /// <summary>
    /// Update entity in db
    /// </summary>
    /// <param name="entity"></param>
    void Update (TEntity entity);

    /// <summary>
    /// Delete entity from db
    /// </summary>
    /// <param name="entityToDelete"></param>
    void Delete (TEntity entityToDelete);
}
