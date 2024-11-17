namespace PieceOfCake.Core.Common.Persistence;

public interface IUnitOfWork : IDisposable
{
    IMeasureUnitRepository MeasureUnitRepository { get; }
    IProductRepository ProductRepository { get; }
    IDishRepository DishRepository { get; }
    IMenuRepository MenuRepository { get; }
    IMealOfTheDayTypeRepository MealOfTheDayTypeRepository { get; }

    Task<int> SaveAsync ();

    //IGenericRepository<TEntity> GetRepositoryByType<TEntity>() where TEntity : Entity;
}
