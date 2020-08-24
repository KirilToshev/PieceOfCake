using PieceOfCake.Shared.ViewModels.Dish.Ingredient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Shared.ViewModels.Dish
{
    public class DishVm
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string State { get; set; }

        public IEnumerable<ReadIngredientVm> Ingredients { get; set; }
    }
}
