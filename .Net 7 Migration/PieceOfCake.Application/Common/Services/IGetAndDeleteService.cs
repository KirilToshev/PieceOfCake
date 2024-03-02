using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Common.Services;

public interface IGetAndDeleteService<TDto, KId>
    where KId : IComparable<KId>
{
    IReadOnlyCollection<TDto> GetAllAsync ();
    Result<TDto> GetByIdAsync (KId id);
    Result DeleteAsync (KId id);
}
