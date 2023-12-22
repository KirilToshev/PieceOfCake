using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Common.Services;

public interface IGetAndDeleteService<TDto, KId>
    where KId : IComparable<KId>
{
    IReadOnlyCollection<TDto> Get ();
    Result<TDto> Get (KId id);
    Result Delete (KId id);
}
