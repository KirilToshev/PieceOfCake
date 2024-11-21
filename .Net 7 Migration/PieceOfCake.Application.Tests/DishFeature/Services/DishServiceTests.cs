using AutoFixture;
using CSharpFunctionalExtensions;
using NSubstitute;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Application.DishFeature.Services;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.DishFeature.Enumerations;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Core.MenuFeature.Entities;
using PieceOfCake.Tests.Common;
using PieceOfCake.Tests.Common.Fakes;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Application.Tests.DishFeature.Services;

public class DishServiceTests : TestsBase
{
    private IUnitOfWork _uowMock;
    private IMealOfTheDayTypeRepository _mealOfTheDayTypeRepoMock;
    private IDishRepository _dishRepoMock;
    private IMeasureUnitRepository _measureUnitRepoMock;
    private IProductRepository _productRepoMock;
    private IMenuRepository _menuRepoMock;
    private IDishFakes _dishFakes;
    private IMealOfTheDayTypeFakes _mealOfTheDayTypeFakes;
    private IIngredientFakes _ingredientFakes;

    public DishServiceTests()
    {
        _uowMock = Substitute.For<IUnitOfWork>();
        _mealOfTheDayTypeRepoMock = Substitute.For<IMealOfTheDayTypeRepository>();
        _measureUnitRepoMock = Substitute.For<IMeasureUnitRepository>();
        _productRepoMock = Substitute.For<IProductRepository>();
        _dishRepoMock = Substitute.For<IDishRepository>();
        _menuRepoMock = Substitute.For<IMenuRepository>();
        _uowMock.MealOfTheDayTypeRepository.Returns(_mealOfTheDayTypeRepoMock);
        _uowMock.MeasureUnitRepository.Returns(_measureUnitRepoMock);
        _uowMock.ProductRepository.Returns(_productRepoMock);
        _uowMock.DishRepository.Returns(_dishRepoMock);
        _uowMock.MenuRepository.Returns(_menuRepoMock);


        _dishFakes = GetRequiredService<IDishFakes>();
        _mealOfTheDayTypeFakes = GetRequiredService<IMealOfTheDayTypeFakes>();
        _ingredientFakes = GetRequiredService<IIngredientFakes>();

        _dishRepoMock.FirstOrDefaultAsync(
            Arg.Any<CancellationToken>(),
            Arg.Any<Expression<Func<Dish, bool>>>())
            .Returns(Task.FromResult(null as Dish));
    }

    [Fact]
    public async Task Get_Should_Return_User_Error_If_Id_Is_Not_Found()
    {
        var notExistingId = Fixture.Create<Guid>();
        _dishRepoMock.GetByIdAsync(Arg.Is(notExistingId), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(null as Dish));

        var sut = new DishService(Resources, _uowMock);
        var result = await sut.GetByIdAsync(notExistingId, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public async Task Get_Should_Return_Dish_If_Id_Is_Found()
    {
        var breakfast = Dish.Create(
        name: TestsConstants.Dishes.BREAKFAST_DISH,
        description: Fixture.Create<string>(),
        servingSize: Fixture.Create<byte>(),
        mealOfTheDayTypes: [_mealOfTheDayTypeFakes.Breakfast ],
        ingredients: [_ingredientFakes.Two_Kilogram_Of_Peppers],
        Resources).Value;

        _dishRepoMock.GetByIdAsync(breakfast.Id, CancellationToken.None)
            .Returns(Task.FromResult(breakfast));

        var sut = new DishService(Resources, _uowMock);
        var result = await sut.GetByIdAsync(breakfast.Id, CancellationToken.None);
        var dto = result.Value;

        Assert.True(result.IsSuccess);
        Assert.Equal(breakfast.Id, dto.Id);
        Assert.Equal(breakfast.Name, dto.Name);
        Assert.Equal(breakfast.Description, dto.Description);
        Assert.Equal(breakfast.DishState.State.ToString(), dto.DishState);
        Assert.Equal(breakfast.ServingSize, dto.ServingSize);
        Assert.Collection(dto.MealOfTheDayTypes,
            mt => 
            {
                var breakfastMealType = breakfast.MealOfTheDayTypes.First();
                Assert.Equal(mt.Id, breakfastMealType.Id);
                Assert.Equal(mt.Name, breakfastMealType.Name);
            });
        Assert.Collection(dto.Ingredients,
            ingredient =>
            {
                var randomIngredient = breakfast.Ingredients
                                        .First(x => x.Product.Name == ingredient.Product.Name);
                Assert.Equal(randomIngredient.Quantity, ingredient.Quantity);
                Assert.Equal(randomIngredient.Product.Id, ingredient.Product.Id);
                Assert.Equal(randomIngredient.MeasureUnit.Id, ingredient.MeasureUnit.Id);
            });
    }

    [Fact]
    public async Task GetAll_Should_Return_Two_Products()
    {
        var breakfast = _dishFakes.Breakfast();
        var lunch = _dishFakes.Lunch();
        _dishRepoMock.GetAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new Dish[] { breakfast, lunch } as IReadOnlyCollection<Dish>));

        var sut = new DishService(Resources, _uowMock);
        var result = await sut.GetAllAsync(CancellationToken.None);

        Assert.True(result.Count == 2);
        Assert.Collection(result,
            product1 =>
            {
                Assert.Equal(breakfast.Id, product1.Id);
                Assert.Equal(breakfast.Name.Value, product1.Name);
            },
            product2 =>
            {
                Assert.Equal(lunch.Id, product2.Id);
                Assert.Equal(lunch.Name.Value, product2.Name);
            });
    }

    [Fact]
    public async Task Create_Should_Succseed_If_Data_Is_Valid()
    {
        //Arrange
        var breakfast = _mealOfTheDayTypeFakes.Breakfast;
        var pepper = _ingredientFakes.Two_Kilogram_Of_Peppers.Product;
        var kg = _ingredientFakes.Two_Kilogram_Of_Peppers.MeasureUnit;
        var createDto = new DishCreateDto
        {
            Name = Fixture.Create<string>(),
            Description = Fixture.Create<string>(),
            ServingSize = 3,
            MealOfTheDayTypes = [new MealOfTheDayTypeDto
            {
                Id = breakfast.Id, 
                Name = breakfast.Name 
            }],
            IngredientsDtos = [new IngredientCreateDto
            {
                Quantity = 2,
                MeasureUnitId = kg.Id,
                ProductId = pepper.Id
            }]
        };

        _measureUnitRepoMock.GetAsync(Arg.Any<CancellationToken>(),
            Arg.Any<Expression<Func<MeasureUnit, bool>>>())
            .Returns(Task.FromResult(new MeasureUnit[] 
            { 
                _ingredientFakes.Two_Kilogram_Of_Peppers.MeasureUnit 
            } as IReadOnlyCollection<MeasureUnit>));

        _productRepoMock.GetAsync(Arg.Any<CancellationToken>(),
            Arg.Any<Expression<Func<Product, bool>>>())
            .Returns(Task.FromResult(new Product[]
            {
                _ingredientFakes.Two_Kilogram_Of_Peppers.Product
            } as IReadOnlyCollection<Product>));

        _mealOfTheDayTypeRepoMock.GetAsync(Arg.Any<CancellationToken>(),
            Arg.Any<Expression<Func<MealOfTheDayType, bool>>>())
            .Returns(Task.FromResult(new MealOfTheDayType[]
            {
                breakfast
            } as IReadOnlyCollection<MealOfTheDayType>));

        //Act
        var sut = new DishService(Resources, _uowMock);
        var result = await sut.CreateAsync(createDto, CancellationToken.None);

        //Assert
        _dishRepoMock.Received(1).Insert(Arg.Any<Dish>());
        _uowMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        Assert.True(result.IsSuccess);
        Assert.Equal(createDto.Name, result.Value.Name);
        Assert.Equal(createDto.Description, result.Value.Description);
        Assert.Equal(createDto.ServingSize, result.Value.ServingSize);
        Assert.Equal(DishState.Draft.ToString() , result.Value.DishState);
    }


    //[Fact]
    //public async Task Update_Should_Return_User_Error_If_Id_Is_Not_Found()
    //{
    //    var notExistingId = Fixture.Create<Guid>();
    //    _mealOfTheDayTypeRepoMock.GetByIdAsync(notExistingId, CancellationToken.None)
    //        .Returns(Task.FromResult(null as MealOfTheDayType));

    //    var sut = new MealOfTheDayTypeService(Resources, _uowMock);

    //    var result = await sut.UpdateAsync(new MealOfTheDayTypeUpdateDto() { Id = notExistingId, Name = Fixture.Create<string>() }, CancellationToken.None);

    //    _mealOfTheDayTypeRepoMock.DidNotReceiveWithAnyArgs().Insert(default);
    //    await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
    //    Assert.True(result.IsFailure);
    //    Assert.Equal($"Element with Id={notExistingId} does not exists.", result.Error);
    //}

    //[Fact]
    //public async Task Update_Should_Succseed_If_Id_Is_Found()
    //{
    //    //Arrange
    //    var id = Fixture.Create<Guid>();
    //    var updatedName = Fixture.Create<string>();
    //    var breakfast = _mealOfTheDayTypeFakes.Breakfast;
    //    var dinner = _mealOfTheDayTypeFakes.Dinner;
    //    _mealOfTheDayTypeMock.UpdateAsync(Arg.Is(updatedName), Arg.Any<IResources>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
    //        .Returns(Result.Success(dinner));
    //    _mealOfTheDayTypeRepoMock.GetByIdAsync(Arg.Is(id), Arg.Any<CancellationToken>())
    //        .Returns(Task.FromResult(breakfast));
    //    var sut = new MealOfTheDayTypeService(Resources, _uowMock);

    //    //Act
    //    var result = await sut.UpdateAsync(new MealOfTheDayTypeUpdateDto() { Id = id, Name = updatedName }, CancellationToken.None);

    //    //Assert
    //    _mealOfTheDayTypeRepoMock.Received(1).Update(Arg.Any<MealOfTheDayType>());
    //    await _uowMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
    //    Assert.True(result.IsSuccess);
    //    Assert.NotNull(result.Value);
    //}

    //[Fact]
    //public async Task Delete_Should_Return_User_Error_If_Id_Is_Not_Found()
    //{
    //    var notExistingId = Fixture.Create<Guid>();
    //    _mealOfTheDayTypeRepoMock.GetByIdAsync(Arg.Is(notExistingId), CancellationToken.None)
    //        .Returns(Task.FromResult(null as MealOfTheDayType));
    //    var sut = new MealOfTheDayTypeService(Resources, _uowMock);

    //    var result = await sut.DeleteAsync(notExistingId, CancellationToken.None);

    //    _mealOfTheDayTypeRepoMock.DidNotReceiveWithAnyArgs().Delete(default);
    //    await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
    //    Assert.True(result.IsFailure);
    //    Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    //}

    //public static IEnumerable<object[]> TestGetPersonItemsData =>
    //    new List<object[]>
    //    {
    //        new object[] { new Dish[] { Substitute.For<Dish>() }, new Menu[0] },
    //        new object[] { new Dish[0], new Menu[] { Substitute.For<Menu>() } },
    //    };

    //[Theory]
    //[MemberData(nameof(TestGetPersonItemsData))]
    //public async Task Delete_Should_Fail_If_MealOfTheDayType_Is_In_Use(
    //    IEnumerable<Dish> dishes,
    //    IEnumerable<Menu> menus)
    //{
    //    var id = Fixture.Create<Guid>();
    //    _mealOfTheDayTypeRepoMock.GetByIdAsync(id, CancellationToken.None)
    //        .Returns(Task.FromResult(_mealOfTheDayTypeMock));
    //    _dishRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Dish, bool>>>(), null)
    //        .Returns(Task.FromResult(dishes as IReadOnlyCollection<Dish>));
    //    _menuRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Menu, bool>>>(), null)
    //        .Returns(Task.FromResult(menus as IReadOnlyCollection<Menu>));
    //    var sut = new MealOfTheDayTypeService(Resources, _uowMock);

    //    var result = await sut.DeleteAsync(id, CancellationToken.None);

    //    _mealOfTheDayTypeRepoMock.DidNotReceiveWithAnyArgs().Delete(default);
    //    await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
    //    Assert.True(result.IsFailure);
    //    Assert.Equal($"{Resources.CommonTerms.MealOfTheDayType} can't be deleted, because it is still being used.", result.Error);
    //}

    //[Fact]
    //public async Task Delete_Should_Succseed_If_Id_Is_Found()
    //{
    //    var id = Fixture.Create<Guid>();
    //    _mealOfTheDayTypeRepoMock.GetByIdAsync(id, CancellationToken.None)
    //        .Returns(Task.FromResult(_mealOfTheDayTypeMock));

    //    _dishRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Dish, bool>>>(), null)
    //        .Returns(Task.FromResult(new Dish[0] as IReadOnlyCollection<Dish>));

    //    _menuRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Menu, bool>>>(), null)
    //        .Returns(Task.FromResult(new Menu[0] as IReadOnlyCollection<Menu>));

    //    var sut = new MealOfTheDayTypeService(Resources, _uowMock);

    //    var result = await sut.DeleteAsync(id, CancellationToken.None);

    //    _mealOfTheDayTypeRepoMock.Received(1).Delete(Arg.Any<MealOfTheDayType>());
    //    await _uowMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
    //    Assert.True(result.IsSuccess);
    //}
}
