using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.Shared.ViewModels.Dish.Ingredient
{
    public class AddIngredientVm
    {
        public float Quantity { get; set; }

        public long MeasureUnitId { get; set; }

        public long ProductId { get; set; }
    }
}
