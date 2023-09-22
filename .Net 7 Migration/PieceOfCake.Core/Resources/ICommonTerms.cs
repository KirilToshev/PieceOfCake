using PieceOfCake.Core.Enumerations;

namespace PieceOfCake.Core.Resources;

public interface ICommonTerms
{
    public string MeasureUnit { get; }
    public string Product { get; }
    public string Dish { get; }
    public string MealOfTheDayType { get; }

    string DishState(DishState state);
    string DayOfWeek(DayOfWeek dayOfWeek);
}
