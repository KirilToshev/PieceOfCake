using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Common.ValueObjects;
using PieceOfCake.Core.IngredientFeature.ValueObjects;
using PieceOfCake.Core.MenuFeature.Entities;

namespace PieceOfCake.Core.DishFeature.Entities;

public class Dish : GuidEntity
{
    private IEnumerable<Ingredient> _ingredients;
    private IEnumerable<Menu> _menus;

    protected Dish ()
    {

    }

    protected Dish (
        Name name,
        string description,
        byte servingSize,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes,
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
    public byte ServingSize { get; protected set; }
    public IEnumerable<MealOfTheDayType> MealOfTheDayTypes { get; protected set; }

    public States.DishState DishState { get; protected set; }

    public virtual IReadOnlyCollection<Ingredient> Ingredients => _ingredients.ToList().AsReadOnly();

    public virtual IReadOnlyCollection<Menu> Menus => _menus.ToList().AsReadOnly();

    public static Result<Dish> Create (
        string name,
        string description,
        byte servingSize,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes,
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

        if (servingSize >= byte.MaxValue)
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.ServingSizeMustBeLessThanByteLimit, x => $"{byte.MaxValue - 1}"));

        if (description.Length > Constants.TEN_THOUSAND)
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.DescriptionExceedsMaxLength, x => x.CommonTerms.Dish, x => $"{Constants.TEN_THOUSAND}"));

        if (!mealOfTheDayTypes.Any())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.DishMustHaveMenuOfTheDayType));

        if (mealOfTheDayTypes.HasUniqueValuesOnly())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.MealOfTheDayTypeAlreadyExists));

        if (!ingredients.Any())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.DishMustHaveIngredients));

        if (ingredients.DistinctBy(x => x.Product).Count() != ingredients.Count())
            return Result.Failure<Dish>(resources.GenereteSentence(x => x.UserErrors.IngredientAlreadyExists));

        return Result.Success(new Dish(nameResult.Value, description, servingSize, mealOfTheDayTypes, ingredients.ToList(), resources));
    }

    public Result<Dish> Update (
        string name,
        string description,
        byte servingSize,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypes,
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
        }).Map(draftState =>
        {
            DishState = draftState;
            return this;
        });
    }

    public Result SubmitForApproval()
    {
        return DishState
            .AwaitingApproval(() => Result.Success())
            .Map(awaitingApprovalState =>
            {
                DishState = awaitingApprovalState;
                return Result.Success();
            });
    }

    public Result Appove ()
    {
        return DishState
            .Active(() => Result.Success())
            .Map(activeState =>
            {
                DishState = activeState;
                return Result.Success();
            });
    }

    public Result Reject ()
    {
        //TODO: Add Rejection entity with Reason inside it.
        return DishState
            .Rejected(() => Result.Success())
            .Map(activeState =>
            {
                DishState = activeState;
                return Result.Success();
            });
    }
}
