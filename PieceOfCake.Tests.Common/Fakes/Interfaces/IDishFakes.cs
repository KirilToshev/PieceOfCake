using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;

namespace PieceOfCake.Tests.Common.Fakes.Interfaces;
public interface IDishFakes
{
    Dish Breakfast (byte? servingSize = null);
    Dish BreakfastLunchAndDinner (byte? servingSize = null);
    Dish Create (string? name = null, string? description = null, byte? servingSize = null, IEnumerable<MealOfTheDayType>? mealOfTheDayTypes = null, IEnumerable<Ingredient>? ingredients = null);
    Dish Dinner (byte? servingSize = null);
    Dish Lunch (byte? servingSize = null);
    Dish LunchAndDinner (string? name = null, byte? servingSize = null);
}
