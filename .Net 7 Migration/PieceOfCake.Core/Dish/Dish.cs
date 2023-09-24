using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.ValueObjects;
using PieceOfCake.Core.Dish.ValueObjects;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Core.Dish;

public class Dish : Entity<Guid>
{
    private IEnumerable<Ingredient> _ingredients;
    private IEnumerable<Menu.Menu> _menus;

    protected Dish ()
    {

    }

    protected Dish (
        Name name,
        string description,
        int servingSize,
        IEnumerable<MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<Ingredient> ingredients,
        IResources resources)
    {
        Name = name;
        Description = description;
        DishState = new States.DraftState(resources);
        _ingredients = ingredients;
        ServingSize = servingSize;
        MealOfTheDayTypes = mealOfTheDayTypes;
    }

    public Name Name { get; protected set; }
    public string Description { get; protected set; }
    public int ServingSize { get; protected set; }
    public IEnumerable<MealOfTheDayType.MealOfTheDayType> MealOfTheDayTypes { get; protected set; }

    public States.DishState DishState { get; protected set; }

    public virtual IReadOnlyCollection<Ingredient> Ingredients { get => _ingredients.ToList().AsReadOnly(); }

    public virtual IReadOnlyCollection<Menu.Menu> Menus { get => _menus.ToList().AsReadOnly(); }

    public static Result<Dish> Create (
        string name,
        string description,
        int servingSize,
        IEnumerable<MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes,
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

        if (!mealOfTheDayTypes.Any())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.DishMustHaveMenuOfTheDayType));

        if (mealOfTheDayTypes.Distinct().Count() != mealOfTheDayTypes.Count())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.MenuOfTheDayTypeAlreadyExists));

        if (!ingredients.Any())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.DishMustHaveIngredients));

        if (ingredients.DistinctBy(x => x.Product).Count() != ingredients.Count())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.IngredientAlreadyExists));

        return Result.Success(new Dish(nameResult.Value, description, servingSize, mealOfTheDayTypes, ingredients.ToList(), resources));
    }

    public Result<Dish> Update (
        string name,
        string description,
        int servingSize,
        IEnumerable<MealOfTheDayType.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<Ingredient> ingredients,
        IResources resources)
    {
        return DishState.Draft(() =>
        {
            var dishResult = Create(name, description, servingSize, mealOfTheDayTypes, ingredients, resources);
            if (dishResult.IsFailure)
                return dishResult;

            Name = dishResult.Value.Name;
            Description = dishResult.Value.Description;
            ServingSize = dishResult.Value.ServingSize;
            MealOfTheDayTypes = dishResult.Value.MealOfTheDayTypes;
            _ingredients = dishResult.Value.Ingredients;

            return Result.Success();
        }).Map(state =>
        {
            DishState = state;
            return this;
        });
    }
}
