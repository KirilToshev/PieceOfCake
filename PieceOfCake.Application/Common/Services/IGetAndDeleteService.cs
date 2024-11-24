using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Common.Services;

public interface IGetAndDeleteService<TDto, KId>
    where KId : IComparable<KId>
{
    Task<IReadOnlyCollection<TDto>> GetAllAsync (CancellationToken cancellationToken);
    Task<Result<TDto>> GetByIdAsync (KId id, CancellationToken cancellationToken);
    Task<Result> DeleteAsync (KId id, CancellationToken cancellationToken);
}
