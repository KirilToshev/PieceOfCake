using AutoFixture;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;

namespace PieceOfCake.Tests.Common.Fakes;

public class DishFakes : BaseFakes
{
    private MealOfTheDayTypeFakes _mealOfTheDayTypeFakes;
    private IngredientFakes _ingredientFakes;

    public DishFakes (IResources resources, IUnitOfWork uowMock)
        : base(resources, uowMock)
    {
        _mealOfTheDayTypeFakes = new MealOfTheDayTypeFakes(_resources, _uowMock);
        _ingredientFakes = new IngredientFakes(_resources, _uowMock);
    }

    public Dish Breakfast (byte servingSize) => Create(
        name: TestsConstants.Dishes.BREAKFAST_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new [] {_mealOfTheDayTypeFakes.Breakfast});

    public Dish Lunch (byte servingSize) => Create(
        name: TestsConstants.Dishes.LUNCH_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new[] { _mealOfTheDayTypeFakes.Lunch });

    public Dish Dinner (byte servingSize) => Create(
        name: TestsConstants.Dishes.DINNER_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new[] { _mealOfTheDayTypeFakes.Dinner });

    public Dish LunchAndDinner (byte servingSize) => Create(
        name: TestsConstants.Dishes.LUNCH_AND_DINNER_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new[] { _mealOfTheDayTypeFakes.Lunch, _mealOfTheDayTypeFakes.Dinner });

    public Dish BreakfastLunchAndDinner (byte servingSize) => Create(
        name: TestsConstants.Dishes.BREAKFAST_LUNCH_AND_DINNER_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new[] { 
            _mealOfTheDayTypeFakes.Breakfast, 
            _mealOfTheDayTypeFakes.Lunch,
            _mealOfTheDayTypeFakes.Dinner });

    public Dish Create (
        string? name = null,
        string? description = null,
        byte? servingSize = null,
        IEnumerable<MealOfTheDayType>? mealOfTheDayTypes = null,
        IEnumerable<Ingredient>? ingredients = null
        )
    {
        if (name is null)
            name = _fixture.Create<string>();
        if (description is null)
            description = _fixture.Create<string>();
        if (mealOfTheDayTypes is null)
            mealOfTheDayTypes = _fixture.RandomListOf(
                _mealOfTheDayTypeFakes.Breakfast,
                _mealOfTheDayTypeFakes.Lunch,
                _mealOfTheDayTypeFakes.Dinner);
        if (ingredients is null)
            ingredients = _fixture.RandomListOf(
                _ingredientFakes.One_Number_Of_Carrots,
                _ingredientFakes.Two_Kilogram_Of_Peppers,
                _ingredientFakes.Three_Litters_Of_Water);

        return Dish.Create(
            name,
            description,
            servingSize ?? _fixture.Create<byte>(),
            mealOfTheDayTypes,
            ingredients,
            _resources).Value;
    }
}
