using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Common.Services;

public interface IGetAndDeleteService<TDto, KId>
    where KId : IComparable<KId>
{
    Task<IReadOnlyCollection<TDto>> GetAllAsync ();
    Task<Result<TDto>> GetByIdAsync (KId id);
    Task<Result> DeleteAsync (KId id);
}
