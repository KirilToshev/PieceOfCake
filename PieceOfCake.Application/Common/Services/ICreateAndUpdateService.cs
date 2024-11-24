using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Common.Services;
public interface ICreateAndUpdateService<TGetDto, TCreateDto, TUpdateDto> 
    : IGetAndDeleteService<TGetDto, Guid>
{
    Task<Result<TGetDto>> CreateAsync (TCreateDto createDto, CancellationToken cancellationToken);
    Task<Result<TGetDto>> UpdateAsync (TUpdateDto updateDto, CancellationToken cancellationToken);
}
