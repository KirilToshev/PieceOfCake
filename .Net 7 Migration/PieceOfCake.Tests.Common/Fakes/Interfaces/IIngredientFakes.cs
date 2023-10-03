using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;

namespace PieceOfCake.Tests.Common.Fakes.Interfaces;
public interface IIngredientFakes
{
    Ingredient One_Number_Of_Carrots { get; }
    Ingredient Three_Litters_Of_Water { get; }
    Ingredient Two_Kilogram_Of_Peppers { get; }

    Ingredient Create (float? quantity = null, MeasureUnit? measureUnit = null, Product? product = null);
}
