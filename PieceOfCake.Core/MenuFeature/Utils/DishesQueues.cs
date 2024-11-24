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

    public void MoveDishAtTheEndOfAllQueues(Dish dishToDequeue)
    {
        foreach (var currenDishesQueue in _dishesQueues.Values)
        {
            var dish = currenDishesQueue.Peek();
            if (dishToDequeue.Equals(dish))
            {
                currenDishesQueue.Dequeue();
                currenDishesQueue.Enqueue(dish);
            }
        }
    }

    public static Result<DishesQueues> Create(
        IEnumerable<Dish> dishes,
        IEnumerable<MealOfTheDayType> mealTypes,
        IResources resources)
    {
        var dishesQueues = dishes.Select(x => x.MealOfTheDayTypes)
            .Aggregate((curr, next) => curr.Union(next))
            .ToDictionary(key => key,
            value => new Queue<Dish>(dishes
                .Where(x => x.MealOfTheDayTypes
                .Select(mt => mt.Id)
                .Contains(value.Id))));

        if (mealTypes.Except(dishesQueues.Keys).Any())
        {
            return Result.Failure<DishesQueues>(resources
                        .GenereteSentence(x => x.UserErrors.NotEnoughDishesOfMenuType,
                            x => string.Join(',', mealTypes.Select(x => x.Name.Value))));
        }

        return new DishesQueues(dishesQueues);
    }
}
