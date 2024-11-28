using PieceOfCake.Core.Common.Persistence;

namespace PieceOfCake.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private bool disposed = false;
    private readonly PocDbContext _context;

    public UnitOfWork(
        PocDbContext context,
        IMeasureUnitRepository MeasureUnitRepository,
        IProductRepository ProductRepository,
        IDishRepository DishRepository,
        IMenuRepository MenuRepository,
        IMealOfTheDayTypeRepository MealOfTheDayTypeRepository)
    {
        _context = context;
    }

    public IMeasureUnitRepository MeasureUnitRepository { get; init; }

    public IProductRepository ProductRepository { get; init; }

    public IDishRepository DishRepository { get; init; }

    public IMenuRepository MenuRepository { get; init; }

    public IMealOfTheDayTypeRepository MealOfTheDayTypeRepository { get; init; }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposed)
        {
            if(disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
