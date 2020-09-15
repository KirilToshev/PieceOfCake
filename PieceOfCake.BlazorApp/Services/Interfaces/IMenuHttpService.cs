using CSharpFunctionalExtensions;
using PieceOfCake.Shared.ViewModels.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services.Interfaces
{
    public interface IMenuHttpService
    {
        Task<Result<IEnumerable<MenuVm>>> GetAllMenus();

        Task<Result<MenuVm>> GetMenuById(long menuId);

        Task<Result<MenuVm>> Create(MenuVm menu);

        Task<Result<MenuVm>> Update(MenuVm menu);

        Task<Result> Delete(long menuId);

        Task<Result<MenuVm>> GenerateDishesList(long menuId);
    }
}
