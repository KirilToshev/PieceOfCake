using PieceOfCake.Core.Common;
using PieceOfCake.Core.Enumerations;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.Entities
{
    public class Dish : Entity
    {
        #pragma warning disable 8618
        #warning Sparation Of Concerns violation
        //This is required to suppress warnings/errors in the default(empty) constructor
        //required by Moq to construct this object in the UnitTests
        protected Dish()
        {
        }

        protected Dish(
            Name name,
            string description
            )
        {
            this.Name = name;
            this.Description = description;
            this.State = DishState.Draft;
            this.Ingredients = new HashSet<Ingredient>();
        }

        public Name Name { get; protected set; }
        public string Description { get; protected set; }
        public DishState State { get; protected set; }
        public virtual ICollection<Ingredient> Ingredients { get; protected set; }
    }
}
