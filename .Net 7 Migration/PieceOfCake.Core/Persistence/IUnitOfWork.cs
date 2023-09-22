namespace PieceOfCake.Core.Persistence;

public interface IUnitOfWork
{
    IMeasureUnitRepository MeasureUnitRepository { get; }
    IProductRepository ProductRepository { get; }
    IDishRepository DishRepository { get; }
    IMenuRepository MenuRepository { get; }
    IMealOfTheDayTypeRepository MealOfTheDayTypeRepository { get; }

    void Save();
    Task<int> SaveAsync();

    //IGenericRepository<TEntity> GetRepositoryByType<TEntity>() where TEntity : Entity;
}
