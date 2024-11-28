using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.Common.Persistence;

namespace PieceOfCake.DAL.Repositories;

public class GenericRepository<TEntity>(PocDbContext context) : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> dbSet = context.Set<TEntity>();

    public virtual void Insert(TEntity entity)
    {
        dbSet.Add(entity);
    }

    public virtual void Delete(object id)
    {
        TEntity entityToDelete = dbSet.Find(id);
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if(context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetAsync(
        CancellationToken cancellationToken, 
        Expression<Func<TEntity, bool>>? filter = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = dbSet;

        if(filter != null)
        {
            query = query.Where(filter);
        }

        foreach(var includeProperty in includes)
        {
            query = query.Include(includeProperty);
        }

        if(orderBy != null)
        {
            return await orderBy(query).ToArrayAsync(cancellationToken);
        }
        else
        {
            return await query.ToArrayAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        return await dbSet.FindAsync(id, cancellationToken);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(
        CancellationToken cancellationToken, 
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = await GetAsync(cancellationToken, filter, null, includes);
        return query.FirstOrDefault();
    }
}
