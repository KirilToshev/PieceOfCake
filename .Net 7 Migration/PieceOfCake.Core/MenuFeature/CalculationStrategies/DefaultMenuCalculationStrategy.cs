using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Calendar;
using PieceOfCake.Core.MenuFeature.Utils;

namespace PieceOfCake.Core.MenuFeature.CalculationStrategies;

public class DefaultMenuCalculationStrategy : IMenuCalculationStrategy
{
    private readonly IResources _resources;

    public DefaultMenuCalculationStrategy (IResources resources)
    {
        _resources = resources;
    }

    public Result<IEnumerable<CalendarItem>> Calculate (
        MenuCalendar calendar,
        IEnumerable<Dish> dishes)
    {
        var queuesResult = DishesQueues.Create(dishes, calendar.MealOfTheDayTypes, _resources);
        if (queuesResult.IsFailure)
            return queuesResult.ConvertFailure<IEnumerable<CalendarItem>>();

        var dishesPerMealTypeQueues = queuesResult.Value;
        var servingsPerDishCounter = dishes.ToDictionary(key => key.Id, value => 0);
       
        // Iterate each day (e.g Mondary, Thusday, etc..)
        foreach (var kvPair in calendar)
        {
            var date = kvPair.Key;

            // Iterate each Meal Type (e.g Breakfast, Lunch, etc...)
            foreach (var mealTypeKvPair in kvPair.Value)
            {
                var mealType = mealTypeKvPair.Key;
                var dishesOfCurrentMealTypeQueue = dishesPerMealTypeQueues[mealType];

                // TODO: It is possible to connect users Ids in the future instead of just a number.
                // Iterate each person/serving.
                for (ushort personIndex = 0; personIndex < mealTypeKvPair.Value.Length; personIndex++)
                {
                    var dish = dishesOfCurrentMealTypeQueue.Peek();
                    calendar[date, mealType, personIndex] = dish;
                    servingsPerDishCounter[dish.Id]++;

                    if (dish.ServingSize < servingsPerDishCounter[dish.Id] + 1)
                    {
                        dishesPerMealTypeQueues.MoveDishAtTheEndOfAllQueues(dish);
                        servingsPerDishCounter[dish.Id] = 0;
                    }
                }
            }
        }

        return Result.Success(calendar.Calendar);
    }
}
