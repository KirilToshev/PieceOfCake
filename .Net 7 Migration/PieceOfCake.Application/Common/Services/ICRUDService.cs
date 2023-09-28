using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Common.Services;

public interface ICRUDService<TEntity, KId>
    where TEntity : Entity<KId>
    where KId : IComparable<KId>
{
    IReadOnlyCollection<TEntity> Get ();
    Result<TEntity> Get (KId id);
    Result Delete (KId id);
}
