using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.MenuFeature.Dtos;

namespace PieceOfCake.Application.MenuFeature.Services;

public interface IMenuService : ICreateAndUpdateService<MenuGetCoreDto, MenuCreateCoreDto, MenuUpdateCoreDto>
{    
    Task<Result<MenuGetCoreDto>> GenerateDishesListAsync (Guid id, CancellationToken cancellationToken);
}
