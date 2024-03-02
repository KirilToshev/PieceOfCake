using CSharpFunctionalExtensions;

namespace PieceOfCake.Application.Common.Services;
public interface ICreateAndUpdateService<TGetDto, TCreateDto, TUpdateDto> 
    : IGetAndDeleteService<TGetDto, Guid>
{
    Result<TGetDto> CreateAsync (TCreateDto createDto);
    Result<TGetDto> UpdateAsync (TUpdateDto updateDto);
}
