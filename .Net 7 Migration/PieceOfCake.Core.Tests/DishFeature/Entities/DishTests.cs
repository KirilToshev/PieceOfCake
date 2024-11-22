using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.DishFeature.Enumerations;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;
using PieceOfCake.Tests.Common;
using PieceOfCake.Tests.Common.Fakes.Interfaces;

namespace PieceOfCake.Core.Tests.DishFeature.Entities;
public class DishTests : TestsBase
{
    private IDishFakes _dishFakes;
    private IMealOfTheDayTypeFakes _mealOfTheDayTypeFakes;
    private IIngredientFakes _ingredientFakes;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IMealOfTheDayTypeRepository> _mealTypeRepoMock;
    private readonly Mock<IMeasureUnitRepository> _measureUnitRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;

    public DishTests ()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _mealTypeRepoMock = new Mock<IMealOfTheDayTypeRepository>();
        _measureUnitRepoMock = new Mock<IMeasureUnitRepository>();
        _productRepoMock = new Mock<IProductRepository>();

        _dishFakes = GetRequiredService<IDishFakes>();
        _mealOfTheDayTypeFakes = GetRequiredService<IMealOfTheDayTypeFakes>();
        _ingredientFakes = GetRequiredService<IIngredientFakes>();
    }

    [SetUp]
    public async Task BeforeEachTest()
    {
        
        _mealTypeRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .ReturnsAsync(null as MealOfTheDayType);
        _measureUnitRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .ReturnsAsync(null as MeasureUnit);
        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(null as Product);
        _uowMock.Setup(x => x.MealOfTheDayTypeRepository)
            .Returns(_mealTypeRepoMock.Object);
        _uowMock.Setup(x => x.MeasureUnitRepository)
            .Returns(_measureUnitRepoMock.Object);
        _uowMock.Setup(x => x.ProductRepository)
            .Returns(_productRepoMock.Object);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Create_Should_Return_User_Error_If_Created_Without_Name (string? name)
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: name, 
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} must have name."));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Create_Should_Return_User_Error_If_Description_Is_Invalid (string? description)
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} must have description."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Description_Exceeds_Symbols_Count_Limit ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: Fixture.CreateStringOfLength(Constants.TEN_THOUSAND + 1),
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} description should not exceed {Constants.TEN_THOUSAND} symbols."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Number_Of_Servings_Is_Less_Than_One ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: 0,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"There should be at least one or more servings in a dish."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Number_Of_Servings_Is_GreatOrEqual_To_255()
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: byte.MaxValue,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"Dish can not have more than {byte.MaxValue - 1} servings."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_MealOfTheDayTypes_Is_Empty ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: new MealOfTheDayType[] { },
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"A dish must have assigned at least " +
            $"one menu of the day type. (Breakfast, Lunch, Dinner, etc...)"));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_MealOfTheDayTypes_Contains_Not_Unique_Values ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes:
            [
                _mealOfTheDayTypeFakes.Breakfast,
                _mealOfTheDayTypeFakes.Breakfast
            ],
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"Meals of the day must be unique."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Ingredients_Is_Empty ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: [],
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"A Dish must have at least one or more ingredients."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Ingredients_Contains_Not_Unique_Products ()
    {
        var validDish = _dishFakes.Create();

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

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"There should be no product duplicates in the current dish." +
            $" Check your products in the ingredients list."));
    }

    [Test]
    public void Create_Should_Succseed_If_Data_Is_Valid ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = Dish.Create(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);
        var dish = dishResult.Value;

        Assert.That(dishResult.IsSuccess);
        Assert.That(dish.Name, Is.EqualTo(validDish.Name));
        Assert.That(dish.Description, Is.EqualTo(validDish.Description));
        Assert.That(dish.ServingSize, Is.EqualTo(validDish.ServingSize));
        Assert.That(dish.MealOfTheDayTypes, Is.EquivalentTo(validDish.MealOfTheDayTypes));
        Assert.That(dish.Ingredients, Is.EquivalentTo(validDish.Ingredients));
        Assert.That(dish.DishState.State, Is.EqualTo(DishState.Draft));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Update_Should_Return_User_Error_If_Created_Without_Name (string? name)
    {
        var validDish = _dishFakes.Create();

        var dishResult = validDish.Update(
            name: name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} must have name."));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Update_Should_Return_User_Error_If_Description_Is_Invalid (string? description)
    {
        var validDish = _dishFakes.Create();

        var dishResult = validDish.Update(
            name: validDish.Name,
            description: description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} must have description."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Description_Exceeds_Symbols_Count_Limit ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = validDish.Update(
            name: validDish.Name,
            description: Fixture.CreateStringOfLength(Constants.TEN_THOUSAND + 1),
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"{Resources.CommonTerms.Dish} description should not exceed {Constants.TEN_THOUSAND} symbols."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Number_Of_Servings_Is_Less_Than_One ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = validDish.Update(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: 0,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"There should be at least one or more servings in a dish."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_MealOfTheDayTypes_Is_Empty ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = validDish.Update(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: new MealOfTheDayType[] { },
            ingredients: validDish.Ingredients,
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"A dish must have assigned at least " +
            $"one menu of the day type. (Breakfast, Lunch, Dinner, etc...)"));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_MealOfTheDayTypes_Contains_Not_Unique_Values ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = validDish.Update(
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

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"Meals of the day must be unique."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Ingredients_Is_Empty ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = validDish.Update(
            name: validDish.Name,
            description: validDish.Description,
            servingSize: validDish.ServingSize,
            mealOfTheDayTypes: validDish.MealOfTheDayTypes,
            ingredients: new Ingredient[] { },
            Resources);

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"A Dish must have at least one or more ingredients."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Ingredients_Contains_Not_Unique_Products ()
    {
        var validDish = _dishFakes.Create();

        var dishResult = validDish.Update(
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

        Assert.That(dishResult.IsFailure);
        Assert.That(dishResult.Error, Is.EqualTo($"There should be no product duplicates in the current dish." +
            $" Check your products in the ingredients list."));
    }

    [Test]
    public void Update_Should_Succseed_If_Data_Is_Valid ()
    {
        var validDish = _dishFakes.Create();
        var expectedDish = _dishFakes.Create();

        var dishResult = validDish.Update(
            name: expectedDish.Name,
            description: expectedDish.Description,
            servingSize: expectedDish.ServingSize,
            mealOfTheDayTypes: expectedDish.MealOfTheDayTypes,
            ingredients: expectedDish.Ingredients,
            Resources);
        var dish = dishResult.Value;

        Assert.That(dishResult.IsSuccess);
        Assert.That(dish.Name, Is.EqualTo(expectedDish.Name));
        Assert.That(dish.Description, Is.EqualTo(expectedDish.Description));
        Assert.That(dish.ServingSize, Is.EqualTo(expectedDish.ServingSize));
        Assert.That(dish.MealOfTheDayTypes, Is.EquivalentTo(expectedDish.MealOfTheDayTypes));
        Assert.That(dish.Ingredients, Is.EquivalentTo(expectedDish.Ingredients));
        Assert.That(dish.DishState.State, Is.EqualTo(DishState.Draft));
    }


    [Test]
    public void SubmitForApproval_Should_Change_Dish_State ()
    {
        var dish = _dishFakes.Create();

        var dishResult = dish.SubmitForApproval();

        Assert.That(dishResult.IsSuccess);
        Assert.That(dish.DishState.State, Is.EqualTo(DishState.AwaitingApproval));
    }

    [Test]
    public void Appove_Should_Change_Dish_State ()
    {
        var dish = _dishFakes.Create();
        dish.SubmitForApproval();

        var dishResult = dish.Appove();

        Assert.That(dishResult.IsSuccess);
        Assert.That(dish.DishState.State, Is.EqualTo(DishState.Active));
    }

    [Test]
    public void Reject_Should_Change_Dish_State ()
    {
        var dish = _dishFakes.Create();
        dish.SubmitForApproval();

        var dishResult = dish.Reject();

        Assert.That(dishResult.IsSuccess);
        Assert.That(dish.DishState.State, Is.EqualTo(DishState.Rejected));
    }
}
