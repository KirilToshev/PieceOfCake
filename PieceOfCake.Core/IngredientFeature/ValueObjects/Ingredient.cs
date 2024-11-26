using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.Core.IngredientFeature.ValueObjects;

public class Ingredient : GuidEntity
{
    protected Ingredient (
        float quantity,
        MeasureUnit measureUnit,
        Product product)
    {
        Quantity = quantity;
        MeasureUnit = measureUnit;
        Product = product;
    }

    public float Quantity { get; protected set; }
    public virtual MeasureUnit MeasureUnit { get; protected set; }
    public virtual Product Product { get; protected set; }
    public virtual Dish Dish { get; protected set; }

    public static Result<Ingredient> Create (float quantity, MeasureUnit measureUnit, Product product, IResources resources)
    {
        if (quantity <= 0)
            return Result.Failure<Ingredient>(resources.GenereteSentence(x => x.UserErrors.QuantityMustBeGraterThanZero));

        return Result.Success(new Ingredient(quantity, measureUnit, product));
    }
}
