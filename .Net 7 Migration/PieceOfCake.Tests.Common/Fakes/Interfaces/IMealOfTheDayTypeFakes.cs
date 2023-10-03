using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Tests.Common.Fakes.Interfaces;
public interface IMealOfTheDayTypeFakes : INameFakes<MealOfTheDayType>
{
    MealOfTheDayType Breakfast { get; }
    MealOfTheDayType Dinner { get; }
    MealOfTheDayType Lunch { get; }
}
