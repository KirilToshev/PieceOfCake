using AutoFixture;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;
using PieceOfCake.Tests.Common.Fakes.Common;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Tests.Common.Fakes;

public class DishFakes : EntityFakes<string, Dish>, IDishFakes
{
    private IMealOfTheDayTypeFakes _mealOfTheDayTypeFakes;
    private IIngredientFakes _ingredientFakes;

    public override Expression<Func<Dish, string>> CacheKey => x => x.Name.Value;

    public DishFakes (
        IResources resources,
        IUnitOfWork uowMock,
        IMealOfTheDayTypeFakes mealOfTheDayTypeFakes,
        IIngredientFakes ingredientFakes)
        : base(resources, uowMock)
    {
        _mealOfTheDayTypeFakes = mealOfTheDayTypeFakes ?? throw new ArgumentNullException(nameof(mealOfTheDayTypeFakes));
        _ingredientFakes = ingredientFakes ?? throw new ArgumentNullException(nameof(ingredientFakes));
    }

    public Dish Breakfast (byte? servingSize = null) => Create(
        name: TestsConstants.Dishes.BREAKFAST_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new[] { _mealOfTheDayTypeFakes.Breakfast });

    public Dish Lunch (byte? servingSize = null) => Create(
        name: TestsConstants.Dishes.LUNCH_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new[] { _mealOfTheDayTypeFakes.Lunch });

    public Dish Dinner (byte? servingSize = null) => Create(
        name: TestsConstants.Dishes.DINNER_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new[] { _mealOfTheDayTypeFakes.Dinner });

    public Dish LunchAndDinner (string? name = null, byte? servingSize = null) => Create(
        name: name ?? TestsConstants.Dishes.LUNCH_AND_DINNER_DISH,
        servingSize: servingSize,
        mealOfTheDayTypes: new[] { _mealOfTheDayTypeFakes.Lunch, _mealOfTheDayTypeFakes.Dinner });

    public Dish BreakfastLunchAndDinner (byte? servingSize = null) => Create(
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
            mealOfTheDayTypes = _fixture.RandomOfList(
                _mealOfTheDayTypeFakes.Breakfast,
                _mealOfTheDayTypeFakes.Lunch,
                _mealOfTheDayTypeFakes.Dinner);
        if (ingredients is null)
            ingredients = _fixture.RandomOfList(
                _ingredientFakes.One_Number_Of_Carrots,
                _ingredientFakes.Two_Kilogram_Of_Peppers,
                _ingredientFakes.Three_Litters_Of_Water);

        var dish = Dish.Create(
            name,
            description,
            servingSize ?? _fixture.Create<byte>(),
            mealOfTheDayTypes,
            ingredients,
            _resources).Value;

        return GetFromCache(dish);
    }
}
