using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.Shared.ViewModels.Dish
{
    public class IngredientVm
    {
        public float Quantity { get; set; }

        public long MeasureUnitId { get; set; }

        public long ProductId { get; set; }
    }
}
