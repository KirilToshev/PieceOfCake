using PieceOfCake.Shared.ViewModels.MeasureUnit;
using PieceOfCake.Shared.ViewModels.Product;

namespace PieceOfCake.Shared.ViewModels.Dish.Ingredient
{
    public class ReadIngredientVm
    {
        public ReadIngredientVm()
        {
            this.MeasureUnit = new MeasureUnitVm();
            this.Product = new ProductVm();
        }

        public long Id { get; set; }

        public float Quantity { get; set; }

        public MeasureUnitVm MeasureUnit { get; set; }

        public ProductVm Product { get; set; }
    }
}
