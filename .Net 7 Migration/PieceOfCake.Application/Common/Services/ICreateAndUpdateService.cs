using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Common.Services;
public interface ICreateAndUpdateService<TGetDto, TCreateDto, TUpdateDto> 
    : IGetAndDeleteService<TGetDto, Guid>
{
    Result<TGetDto> Create (TCreateDto createDto);
    Result<TGetDto> Update (TUpdateDto updateDto);
}
