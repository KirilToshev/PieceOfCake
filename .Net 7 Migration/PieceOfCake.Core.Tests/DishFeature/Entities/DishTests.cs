using NUnit.Framework;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;
using PieceOfCake.Tests.Common;
using PieceOfCake.Tests.Common.Fakes.Interfaces;

namespace PieceOfCake.Core.Tests.DishFeature.Entities;
public class DishTests : TestsBase
{
    private IDishFakes _dishFakes;
    private IMealOfTheDayTypeFakes _mealOfTheDayTypeFakes;
    private IIngredientFakes _ingredientFakes;

    public DishTests ()
    {
        _dishFakes = GetRequiredService<IDishFakes>();
        _mealOfTheDayTypeFakes = GetRequiredService<IMealOfTheDayTypeFakes>();
        _ingredientFakes = GetRequiredService<IIngredientFakes>();
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Create_Should_Return_User_Error_If_Created_Without_Name (string name)
    {
        var validDish = _dishFakes.Breakfast();

        var dishResult = Dish.Create(
            name: name, 
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.IsTrue(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} must have name."));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Create_Should_Return_User_Error_If_Description_Is_Invalid (string description)
    {
        var validDish = _dishFakes.Breakfast();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.IsTrue(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} must have description."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Description_Exceeds_Symbols_Count_Limit ()
    {
        var validDish = _dishFakes.Breakfast();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: Fixture.CreateStringOfLength(Constants.TEN_THOUSAND + 1),
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.IsTrue(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} description should not exceed {Constants.TEN_THOUSAND} symbols."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Number_Of_Servings_Is_Less_Than_One ()
    {
        var validDish = _dishFakes.Breakfast();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: 0,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.IsTrue(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"There should be at least one or more servings in a dish."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_MealOfTheDayTypes_Is_Empty ()
    {
        var validDish = _dishFakes.Breakfast();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: new MealOfTheDayType[] { },
            ingredients: validDish.Ingredients,
            Resources);

        Assert.IsTrue(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"A dish must have assigned at least " +
            $"one menu of the day type. (Breakfast, Lunch, Dinner, etc...)"));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_MealOfTheDayTypes_Contains_Not_Unique_Values ()
    {
        var validDish = _dishFakes.Breakfast();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: new MealOfTheDayType[] 
            {
                _mealOfTheDayTypeFakes.Breakfast,
                _mealOfTheDayTypeFakes.Breakfast
            },
            ingredients: validDish.Ingredients,
            Resources);

        Assert.IsTrue(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"Meals of the day must be unique."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Ingredients_Is_Empty ()
    {
        var validDish = _dishFakes.Breakfast();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: new Ingredient[] { },
            Resources);

        Assert.IsTrue(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"A Dish must have at least one or more ingredients."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Ingredients_Contains_Not_Unique_Products ()
    {
        var validDish = _dishFakes.Breakfast();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: new Ingredient[] 
            {
                _ingredientFakes.One_Number_Of_Carrots,
                _ingredientFakes.One_Number_Of_Carrots
            },
            Resources);

        Assert.IsTrue(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"There should be no product duplicates in the current dish." +
            $" Check your products in the ingredients list."));
    }

    //TODO: Write Update tests

    //TODO: Write State transition tests
}
