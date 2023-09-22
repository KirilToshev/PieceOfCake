using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;

namespace PieceOfCake.Core.Entities;

public class Dish : Entity<Guid>
{
    private IEnumerable<Ingredient> _ingredients;
    private IEnumerable<Menu> _menus;

    protected Dish ()
    {
        
    }

    protected Dish (
        Name name,
        string description,
        int servingSize,
        MealOfTheDayType mealOfTheDayType,
        IEnumerable<Ingredient> ingredients,
        IResources resources)
    {
        Name = name;
        Description = description;
        DishState = new States.DraftState(resources);
        _ingredients = ingredients;
        ServingSize = servingSize;
        MealOfTheDayType = mealOfTheDayType;
    }

    public Name Name { get; protected set; }
    public string Description { get; protected set; }
    public int ServingSize { get; protected set; }
    public MealOfTheDayType MealOfTheDayType { get; protected set; }

    public States.DishState DishState { get; private set; }

    public virtual IReadOnlyCollection<Ingredient> Ingredients { get => _ingredients.ToList().AsReadOnly(); }

    public virtual IReadOnlyCollection<Menu> Menus { get => _menus.ToList().AsReadOnly(); }

    public static Result<Dish> Create(
        string name, 
        string description,
        int servingSize,
        MealOfTheDayType mealOfTheDayType,
        IEnumerable<Ingredient> ingredients,
        IResources resources)
    {
        var nameResult = Name.Create(name, resources, x => x.CommonTerms.Dish, Constants.FIFTY, Constants.TWO);
        if (nameResult.IsFailure)
            return nameResult.ConvertFailure<Dish>();

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.DescriptionIsMandatory, x => x.CommonTerms.Dish));

        if (servingSize < 1)
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.ServingSizeMustBeGraterThanOne));

        if (description.Length > Constants.FIFTY_THOUSAND)
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.DescriptionExceedsMaxLength, x => x.CommonTerms.Dish, x => Constants.FIFTY_THOUSAND.ToString()));

        if (!ingredients.Any())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.DishMustHaveIngredients));

        if(ingredients.DistinctBy(x => x.Product).Count() != ingredients.Count())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.IngredientAlreadyExists));

        return Result.Success(new Dish(nameResult.Value, description, servingSize, mealOfTheDayType, ingredients.ToList(), resources));
    }

    public Result<Dish> Update (
        string name,
        string description,
        int servingSize,
        MealOfTheDayType mealOfTheDayType,
        IEnumerable<Ingredient> ingredients,
        IResources resources)
    {
        return this.DishState.Draft(() =>
        {
            var dishResult = Create(name, description, servingSize, mealOfTheDayType, ingredients, resources);
            if (dishResult.IsFailure)
                return dishResult;

            Name = dishResult.Value.Name;
            Description = dishResult.Value.Description;
            ServingSize = dishResult.Value.ServingSize;
            MealOfTheDayType = dishResult.Value.MealOfTheDayType;
            _ingredients = dishResult.Value.Ingredients;

            return Result.Success();
        }).Map(state =>
        {
            DishState = state;
            return this;
        });
    }
}
