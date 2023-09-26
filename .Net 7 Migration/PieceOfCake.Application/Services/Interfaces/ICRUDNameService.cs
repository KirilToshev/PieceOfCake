using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Services.Interfaces;
public interface ICRUDNameService<TEntity, KId> : ICRUDService<TEntity, KId>
    where TEntity : Entity<KId>
    where KId : IComparable<KId>
{
    Result<TEntity> Create (string name);
    Result<TEntity> Update (KId id, string name);
}
