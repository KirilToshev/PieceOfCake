using CSharpFunctionalExtensions;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Menu;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services
{
    public class MenuHttpService : HttpRequestServiceBase, IMenuHttpService
    {
        public MenuHttpService(HttpClient httpClient)
            :base(httpClient)
        {
        }

        public async Task<Result<IEnumerable<MenuVm>>> GetAllMenus()
        {
            return await base.HandleGet<IEnumerable<MenuVm>>($"api/menu");
        }

        public async Task<Result<MenuVm>> GetMenuById(long menuId)
        {
            return await base.HandleGet<MenuVm>($"api/menu/{menuId}");
        }

        public async Task<Result<MenuVm>> Create(MenuVm menu)
        {
            return await base.HandlePost<MenuVm>($"api/menu", menu);
        }

        public async Task<Result<MenuVm>> Update(MenuVm menu)
        {
            return await base.HandlePut<MenuVm>($"api/menu/{menu.Id}", menu);
        }

        public async Task<Result> Delete(long menuId)
        {
            return await base.HandleDelete($"api/menu/{menuId}");
        }

        public async Task<Result<MenuVm>> GenerateDishesList(long menuId)
        {
            return await base.HandlePatch<MenuVm>($"api/menu/{menuId}");
        }
    }
}
