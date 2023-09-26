using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Services.Interfaces;

public interface ICRUDService<TEntity, KId>
    where TEntity : Entity<KId>
    where KId : IComparable<KId>
{
    IReadOnlyCollection<TEntity> Get ();
    Result<TEntity> Get (KId id);
    Result Delete (KId id);
}
