using CSharpFunctionalExtensions;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Application.Common.Services;

public interface IGetAndDeleteService<TDto, KId>
    where KId : IComparable<KId>
{
    Task<IReadOnlyCollection<TDto>> GetAllAsync ();
    Result<TDto> GetByIdAsync (KId id);
    Result DeleteAsync (KId id);
}
