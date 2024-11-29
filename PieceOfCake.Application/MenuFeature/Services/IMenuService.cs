using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.MenuFeature.Dtos;
using PieceOfCake.Core.MenuFeature.Entities;

namespace PieceOfCake.Application.MenuFeature.Services;

public interface IMenuService : ICreateAndUpdateService<MenuGetDto, MenuCreateCoreDto, MenuUpdateCoreDto>
{    
    Task<Result<Menu>> GenerateDishesListAsync (Guid id, CancellationToken cancellationToken);
}
