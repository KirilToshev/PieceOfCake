using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using PieceOfCake.BlazorApp.Services.Interfaces;
using PieceOfCake.Shared.ViewModels.Product;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Components.Product
{
    public class EditProductDialogBase : DialogBase<ProductVm>
    {
        public bool IsLoading { get; set; }

        [Inject]
        public IProductHttpService ProductService { get; set; }

        public override string Title => "Update Product";

        public override async Task HandleValidSubmit()
        {
            await base.HandleValidSubmit(ProductService.UpdateProduct);
        }

        public void Show(ProductVm product)
        {
            base.Show();
            this.Item = product;
        }
    }
}
