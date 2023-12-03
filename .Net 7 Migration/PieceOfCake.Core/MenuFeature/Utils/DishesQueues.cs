using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Core.MenuFeature.Utils;

public class DishesQueues
{
    private readonly IDictionary<MealOfTheDayType, Queue<Dish>> _dishesQueues;

    private DishesQueues (IDictionary<MealOfTheDayType, Queue<Dish>> dishesQueues)
    {
        _dishesQueues = dishesQueues;
    }

    public Queue<Dish> this[MealOfTheDayType mealOfTheDayType]
    {
        get => _dishesQueues[mealOfTheDayType];
    }

    public static Result<DishesQueues> Create(IEnumerable<Dish> dishes, IResources resources)
    {
        var dishesQueues = new Dictionary<MealOfTheDayType, Queue<Dish>>();
        foreach (var dish in dishes)
        {
            foreach (var mealType in dish.MealOfTheDayTypes)
            {
                if (!dishesQueues.ContainsKey(mealType))
                {
                    var dishesOfCurrentMealTypeQueue = new Queue<Dish>(dishes
                    .Where(x => x.MealOfTheDayTypes
                    .Select(mt => mt.Id)
                    .Contains(mealType.Id)));

                    if (!dishesOfCurrentMealTypeQueue.Any())
                        return Result.Failure<DishesQueues>(resources
                            .GenereteSentence(x => x.UserErrors.NotEnoughDishesOfMenuType,
                                x => mealType.Name));

                    dishesQueues.Add(mealType, dishesOfCurrentMealTypeQueue);
                }
            }
        }

        return new DishesQueues(dishesQueues);
    }
}
