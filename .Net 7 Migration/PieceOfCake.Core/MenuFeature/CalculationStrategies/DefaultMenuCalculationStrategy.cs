using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Calendar;

namespace PieceOfCake.Core.MenuFeature.CalculationStrategies;

internal class DefaultMenuCalculationStrategy : IMenuCalculationStrategy
{
    private Dictionary<Guid, int> _servingsPerDishCounter ;

    public Result<IEnumerable<CalendarItem>> Calculate (
        MenuCalendar calendar,
        IEnumerable<Dish> dishes,
        IResources resources)
    {
        _servingsPerDishCounter = dishes.ToDictionary(key => key.Id, value => 0);
        var dishIndex = 0;

        // Iterate each day (e.g Mondary, Thusday, etc..)
        foreach (var kvPair in calendar)
        {
            var date = kvPair.Key;

            // Iterate each Meal Type (e.g Breakfast, Lunch, etc...)
            foreach (var mealType in kvPair.Value)
            {
                Dish? dish = null;
                var mealTypeId = mealType.Key;
                var dishesOfCurrentMealType = dishes
                    .Where(x => x.MealOfTheDayTypes
                    .Select(mt => mt.Id)
                    .Contains(mealTypeId))
                    .ToArray();
                if (!dishesOfCurrentMealType.Any())
                    return Result.Failure<IEnumerable<CalendarItem>>(resources
                        .GenereteSentence(x => x.UserErrors.NotEnoughDishesOfMenuType,
                            x => calendar.MealOfTheDayTypes[mealTypeId].Name));

                // TODO: It is possible to connect users Ids in the funture instead of number.
                // Iterate each person.
                for (ushort personIndex = 0; personIndex < mealType.Value.Length; personIndex++)
                {
                    dish = dishesOfCurrentMealType[dishIndex];
                    _servingsPerDishCounter[dish.Id]++;
                    if (dish.ServingSize == _servingsPerDishCounter[dish.Id])
                    {
                        _servingsPerDishCounter.Remove(dish.Id);
                        if (!_servingsPerDishCounter.Any())
                        {
                            _servingsPerDishCounter = dishes.ToDictionary(key => key.Id, value => 0);
                        }

                        dishIndex++;
                        if (dishIndex >= dishesOfCurrentMealType.Length)
                        {
                            dishIndex = 0;
                        }
                    }
                    calendar[date, mealTypeId, personIndex] = dish;
                }
            }
        }

        return Result.Success(calendar.Calendar);
    }
}
