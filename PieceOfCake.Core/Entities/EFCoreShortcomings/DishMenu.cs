using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.Entities.EFCoreShortcomings
{
    #pragma warning disable 8618
    #warning Sparation Of Concerns violation
    //This entire class only exists, because of EF Core is not able to identify Many-to-many realtionship in any other way.
    //It has nothing to do with application domain logic.
    //Check for possible improvement at https://github.com/dotnet/efcore/issues/1368
    public class DishMenu
    {
        public DishMenu()
        {
        }

        public long DishId { get; set; }

        public long MenuId { get; set; }

        public virtual Dish Dish { get; set; }

        public virtual Menu Menu { get; set; }
    }
}
