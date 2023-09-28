using PieceOfCake.Core.DishFeature.Enumerations;

namespace PieceOfCake.Core.Common.Resources;

public interface ICommonTerms
{
    public string MeasureUnit { get; }
    public string Product { get; }
    public string Dish { get; }
    public string MealOfTheDayType { get; }

    string DishState (DishState state);
    string DayOfWeek (DayOfWeek dayOfWeek);
}
