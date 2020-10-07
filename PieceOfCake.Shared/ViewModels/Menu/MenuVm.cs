using PieceOfCake.Shared.ViewModels.Dish;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Shared.ViewModels.Menu
{
    public class MenuVm
    {
        public MenuVm()
        {
            this.Dishes = new HashSet<DishVm>();
            this.DishesPerDay = new Dictionary<string, IEnumerable<DishVm>>();
        }

        public long Id { get; set; }
        public byte ServingsPerDay { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IEnumerable<DishVm> Dishes { get; set; }
        public Dictionary<string, IEnumerable<DishVm>> DishesPerDay { get; set; }
    }
}
