﻿using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Core.Entities
{
    public class Ingredient : Common.Entity
    {
        //This is required to suppress warnings/errors in the default(empty) constructor
        //required by Moq to construct this object in the UnitTests
        #pragma warning disable 8618
        protected Ingredient()
        {
        }

        protected Ingredient(
            float quantity,
            MeasureUnit measureUnit,
            Product product)
        {
            this.Quantity = quantity;
            this.MeasureUnit = measureUnit;
            this.Product = product;
        }

        public float Quantity { get; protected set; }
        public virtual MeasureUnit MeasureUnit { get; protected set; }
        public virtual Product Product { get; protected set; }
        public virtual Dish Dish { get; protected set; }

        public static Result<Ingredient> Create(float quantity, MeasureUnit measureUnit, Product product, IResources resources)
        {
            if (quantity <= 0)
                return Result.Failure<Ingredient>(resources.GenereteSentence(x => x.UserErrors.QuantityMustBeGraterThanZero));

            return Result.Success(new Ingredient(quantity, measureUnit, product));
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Ingredient other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetRealType() != other.GetRealType())
                return false;

            return Quantity == other.Quantity
                && MeasureUnit == other.MeasureUnit
                && Product == other.Product;
        }

        public override int GetHashCode()
        {
            return Quantity.GetHashCode() ^ 601
                | MeasureUnit.GetHashCode() ^ 432
                & Product.GetHashCode() ^ 797;
        }
    }
}
