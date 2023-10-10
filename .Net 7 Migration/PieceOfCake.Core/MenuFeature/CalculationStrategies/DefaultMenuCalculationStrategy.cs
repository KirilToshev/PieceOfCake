using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Calendar;

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
        var servingsPerDishCounter = dishes.ToDictionary(key => key.Id, value => 0);
        var mealTypeIndexConter = calendar.MealOfTheDayTypes.ToDictionary(key => key.Key, value => 0);
       
        // Iterate each day (e.g Mondary, Thusday, etc..)
        foreach (var kvPair in calendar)
        {
            var date = kvPair.Key;

            // Iterate each Meal Type (e.g Breakfast, Lunch, etc...)
            foreach (var mealType in kvPair.Value)
            {
                var mealTypeId = mealType.Key;
                var dishesOfCurrentMealType = dishes
                    .Where(x => x.MealOfTheDayTypes
                    .Select(mt => mt.Id)
                    .Contains(mealTypeId))
                    .ToArray();
                if (!dishesOfCurrentMealType.Any())
                    return Result.Failure<IEnumerable<CalendarItem>>(_resources
                        .GenereteSentence(x => x.UserErrors.NotEnoughDishesOfMenuType,
                            x => calendar.MealOfTheDayTypes[mealTypeId].Name));

                // TODO: It is possible to connect users Ids in the future instead of just a number.
                // Iterate each person.
                for (ushort personIndex = 0; personIndex < mealType.Value.Length; personIndex++)
                {
                    var dish = dishesOfCurrentMealType[mealTypeIndexConter[mealTypeId]];
                    servingsPerDishCounter[dish.Id]++;
                    if (dish.ServingSize == servingsPerDishCounter[dish.Id])
                    {
                        servingsPerDishCounter.Remove(dish.Id);
                        if (!servingsPerDishCounter.Any())
                        {
                            servingsPerDishCounter = dishes.ToDictionary(key => key.Id, value => 0);
                        }

                        mealTypeIndexConter[mealTypeId]++;
                        if (mealTypeIndexConter[mealTypeId] >= dishesOfCurrentMealType.Length)
                        {
                            mealTypeIndexConter[mealTypeId] = 0;
                            //servingsPerDishCounter = servingsPerDishCounter
                            //    .Concat(dishesOfCurrentMealType
                            //    .ToDictionary(key => key.Id, value => 0))
                            //    .ToDictionary(kv => kv.Key, kv => kv.Value);
                        }
                    }
                    calendar[date, mealTypeId, personIndex] = dish;
                }
            }
        }

        return Result.Success(calendar.Calendar);
    }

    private class DishCalculations
    {
        private Dish _dish;
        private int _servingsCounter;

        public DishCalculations (Dish dish)
        {
            _dish = dish;
            _servingsCounter = _dish.ServingSize;
        }

        public bool HasNoMoreServings => _servingsCounter == 0;
        public MealOfTheDayType LastServedIn { get; private set; }
        public Dish ServeDish(MealOfTheDayType mealOfTheDayType)
        {
            if (HasNoMoreServings)
                throw new InvalidOperationException("No more servings left");

            LastServedIn = mealOfTheDayType;
            _servingsCounter--;
            return _dish;
        }

        public bool CanBeServedIn(MealOfTheDayType mealOfTheDayType) 
        {
            
        }
    }
}
