using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Core.MenuFeature.Calendar;

public class MenuSummary
{
    public int TotalServingsCount { get; }

    public IDictionary<Dish, int> TotalDishesCounter { get; }

    public IDictionary<Product,  MeasureUnit> TotalProductsCounter { get; }
}
