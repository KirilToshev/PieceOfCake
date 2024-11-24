using System.Linq.Expressions;
using AutoFixture;
using NSubstitute;
using PieceOfCake.Application.MenuFeature.Dtos;
using PieceOfCake.Application.MenuFeature.Services;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Entities;
using PieceOfCake.Tests.Common.Fakes.Interfaces;

namespace PieceOfCake.Application.Tests.MenuFeature.Services;

public class MenuServiceTests : TestsBase
{
    private IUnitOfWork _uowMock;
    private IMealOfTheDayTypeRepository _mealOfTheDayTypeRepoMock;
    private IDishRepository _dishRepoMock;
    private IMenuRepository _menuRepoMock;
    private IDishFakes _dishFakes;
    private IMealOfTheDayTypeFakes _mealOfTheDayTypeFakes;
    private Menu _menuFake;

    public MenuServiceTests()
    {
        _uowMock = Substitute.For<IUnitOfWork>();
        _mealOfTheDayTypeRepoMock = Substitute.For<IMealOfTheDayTypeRepository>();
        _dishRepoMock = Substitute.For<IDishRepository>();
        _menuRepoMock = Substitute.For<IMenuRepository>();

        _uowMock.MealOfTheDayTypeRepository.Returns(_mealOfTheDayTypeRepoMock);
        _uowMock.DishRepository.Returns(_dishRepoMock);
        _uowMock.MenuRepository.Returns(_menuRepoMock);

        _dishFakes = GetRequiredService<IDishFakes>();
        _mealOfTheDayTypeFakes = GetRequiredService<IMealOfTheDayTypeFakes>();

        var breakfastMealType = _mealOfTheDayTypeFakes.Breakfast;
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(0);
        ushort numberOfPeople = 1;
        var mealTypes = new MealOfTheDayType[]
        {
            breakfastMealType
        };
        _menuFake = Menu.Create(startDate, endDate, numberOfPeople, mealTypes, Resources).Value;
    }

    [Fact]
    public async Task Get_Should_Return_Error_If_Id_Is_Not_Found()
    {
        //Arrange
        var unknownId = Fixture.Create<Guid>();
        _menuRepoMock.GetByIdAsync(Arg.Is(unknownId), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(null as Menu));

        //Act
        var sut = new MenuService(Resources, _uowMock);
        var result = await sut.GetByIdAsync(unknownId, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"Element with Id={unknownId} does not exists.", result.Error);
    }

    [Fact]
    public async Task Get_Should_Return_Menu_If_Id_Is_Found()
    {
        //Arrange
        var id = Fixture.Create<Guid>();
        
        _menuRepoMock.GetByIdAsync(Arg.Is(id), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(_menuFake));

        _dishRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Dish, bool>>>())
            .Returns(Task.FromResult(new Dish[] { _dishFakes.Breakfast() } as IReadOnlyCollection<Dish>));

        _mealOfTheDayTypeRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<MealOfTheDayType, bool>>>())
            .Returns(Task.FromResult(new MealOfTheDayType[] { _mealOfTheDayTypeFakes.Breakfast } as IReadOnlyCollection<MealOfTheDayType>));

        //The _menuFake is used, because NSubstitute can only mock vitual methods on classes.
        await _menuFake.GenerateCalendar(_dishRepoMock, Resources, CancellationToken.None);

        //Act
        var sut = new MenuService(Resources, _uowMock);
        var result = await sut.GetByIdAsync(id, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        var dto = result.Value;
        Assert.Equal(dto.StartDate, _menuFake.Duration.StartDate);
        Assert.Equal(dto.EndDate, _menuFake.Duration.EndDate);
        Assert.Equal(1, _menuFake.Duration.DaysDifference);
        Assert.Equal(dto.NumberOfPeople, _menuFake.NumberOfPeople);
        Assert.Collection(dto.MealOfTheDayTypes,
            mt =>
            {
                var expected = _menuFake.MealOfTheDayTypes.First();
                Assert.Equal(expected.Id, mt.Id);
                Assert.Equal(expected.Name, mt.Name);
            });
        Assert.Collection(dto.CalendarItems,
            ci =>
            {
                var expectedCalendarItem = _menuFake.Calendar.First();
                var expectedMealOfTheDayType = expectedCalendarItem.MealOfTheDayTypes.First();
                var exptectedDish = _menuFake.Dishes.First();
                Assert.Equal(expectedCalendarItem.Date, ci.Date);
                var mealTypeResult = ci.MealOfTheDayTypeDtos.First();
                Assert.Equal(expectedMealOfTheDayType.Id, mealTypeResult.Id);
                Assert.Equal(_mealOfTheDayTypeFakes.Breakfast.Name, mealTypeResult.Name);
                var dishResult = mealTypeResult.Dishes.First();
                Assert.Equal(exptectedDish.Id, dishResult.Id);
                Assert.Equal(exptectedDish.Name, dishResult.Name);
            });
    }

    [Fact]
    public async Task Create_Should_Return_Error_If_Data_Is_Invalid()
    {
        //Arrange
        var createDto = new MenuCreateDto
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            NumberOfPeople = 1,
            MealOfTheDayTypes = []
        };

        //Act
        var sut = new MenuService(Resources, _uowMock);
        var result = await sut.CreateAsync(createDto, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"It is impossible to have a menu without at least one meal type.", result.Error);
        _menuRepoMock.DidNotReceiveWithAnyArgs().Insert(default);
        await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
    }

    [Fact]
    public async Task Create_Should_Succseed_If_Data_Is_Valid()
    {
        //Arrange
        var createDto = new MenuCreateDto
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            NumberOfPeople = 1,
            MealOfTheDayTypes = [_mealOfTheDayTypeFakes.Breakfast.Id]
        };

        _mealOfTheDayTypeRepoMock.GetAsync(
            Arg.Any<CancellationToken>(),
            Arg.Any<Expression<Func<MealOfTheDayType, bool>>>())
            .Returns(Task.FromResult(new MealOfTheDayType[] { _mealOfTheDayTypeFakes.Breakfast }
            as IReadOnlyCollection<MealOfTheDayType>));

        //Act
        var sut = new MenuService(Resources, _uowMock);
        var result = await sut.CreateAsync(createDto, CancellationToken.None);

        //Assert
        _menuRepoMock.Received(1).Insert(Arg.Any<Menu>());
        await _uowMock.Received(1).SaveAsync(default);
        Assert.True(result.IsSuccess);
        var resultDto = result.Value;
        Assert.Equal(DateOnly.FromDateTime(createDto.StartDate),  resultDto.StartDate);
        Assert.Equal(DateOnly.FromDateTime(createDto.EndDate),  resultDto.EndDate);
        Assert.Equal(2,  resultDto.DaysDifference);
        Assert.Equal(createDto.NumberOfPeople,  resultDto.NumberOfPeople);
        Assert.Collection(resultDto.MealOfTheDayTypes,
            mt =>
            {
                Assert.Equal(_mealOfTheDayTypeFakes.Breakfast.Id, mt.Id);
                Assert.Equal(_mealOfTheDayTypeFakes.Breakfast.Name, mt.Name);
            });
    }

    [Fact]
    public async Task Update_Should_Return_Error_If_Id_Is_Not_Found()
    {
        //Arrange
        var unknownId = Fixture.Create<Guid>();
        var updateDto = new MenuUpdateDto
        {
            Id = unknownId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            NumberOfPeople = 1,
            MealOfTheDayTypes = [_mealOfTheDayTypeFakes.Breakfast.Id]
        };
        _menuRepoMock.GetByIdAsync(Arg.Is(unknownId), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(null as Menu));

        //Act
        var sut = new MenuService(Resources, _uowMock);
        var result = await sut.UpdateAsync(updateDto, CancellationToken.None);

        //Assert
        _menuRepoMock.DidNotReceiveWithAnyArgs().Insert(default);
        await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
        Assert.True(result.IsFailure);
        Assert.Equal($"Element with Id={unknownId} does not exists.", result.Error);
    }

    [Fact]
    public async Task Update_Should_Succseed_If_Data_Is_Valid()
    {
        //Arrange
        var updateDto = new MenuUpdateDto
        {
            Id = Guid.NewGuid(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            NumberOfPeople = 1,
            MealOfTheDayTypes = [_mealOfTheDayTypeFakes.Breakfast.Id]
        };

        var menuFake = Menu.Create(
            startDate: updateDto.StartDate.AddDays(-1),
            endDate: updateDto.EndDate.AddDays(-1),
            numberOfPeople: 2,
            mealOfTheDayTypes: [_mealOfTheDayTypeFakes.Dinner],
            Resources).Value;

        _menuRepoMock
            .GetByIdAsync(Arg.Is(updateDto.Id), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(menuFake));

        _mealOfTheDayTypeRepoMock.GetAsync(
            Arg.Any<CancellationToken>(),
            Arg.Any<Expression<Func<MealOfTheDayType, bool>>>())
            .Returns(Task.FromResult(new MealOfTheDayType[] { _mealOfTheDayTypeFakes.Breakfast }
            as IReadOnlyCollection<MealOfTheDayType>));

        //Act
        var sut = new MenuService(Resources, _uowMock);
        var result = await sut.UpdateAsync(updateDto, CancellationToken.None);

        //Assert
        await _menuRepoMock.Received(1)
            .GetByIdAsync(Arg.Is(updateDto.Id), Arg.Any<CancellationToken>());
        _menuRepoMock.Received(1).Update(Arg.Any<Menu>());
        await _uowMock.Received(1).SaveAsync(default);
        Assert.True(result.IsSuccess);
        var resultDto = result.Value;
        Assert.Equal(DateOnly.FromDateTime(updateDto.StartDate), resultDto.StartDate);
        Assert.Equal(DateOnly.FromDateTime(updateDto.EndDate), resultDto.EndDate);
        Assert.Equal(2, resultDto.DaysDifference);
        Assert.Equal(updateDto.NumberOfPeople, resultDto.NumberOfPeople);
        Assert.Collection(resultDto.MealOfTheDayTypes,
            mt =>
            {
                Assert.Equal(_mealOfTheDayTypeFakes.Breakfast.Id, mt.Id);
                Assert.Equal(_mealOfTheDayTypeFakes.Breakfast.Name, mt.Name);
            });
    }

    [Fact]
    public async Task Delete_Should_Return_Error_If_Id_Is_Not_Found()
    {
        //Arrange
        var unknownId = Fixture.Create<Guid>();
        _menuRepoMock.GetByIdAsync(Arg.Is(unknownId), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(null as Menu));

        //Act
        var sut = new MenuService(Resources, _uowMock);
        var result = await sut.DeleteAsync(unknownId, CancellationToken.None);

        //Assert
        _menuRepoMock.DidNotReceiveWithAnyArgs().Delete(default);
        await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
        Assert.True(result.IsFailure);
        Assert.Equal($"Element with Id={unknownId} does not exists.", result.Error);
    }

    [Fact]
    public async Task Delete_Should_Succseed_If_Id_Is_Found()
    {
        //Arrange
        var id = Fixture.Create<Guid>();

        _menuRepoMock
            .GetByIdAsync(Arg.Is(id), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Substitute.For<Menu>()));

        //Act
        var sut = new MenuService(Resources, _uowMock);
        var result = await sut.DeleteAsync(id, CancellationToken.None);

        //Assert
        await _menuRepoMock.Received(1)
            .GetByIdAsync(Arg.Is(id), Arg.Any<CancellationToken>());
        _menuRepoMock.Received(1).Delete(Arg.Any<Menu>());
        await _uowMock.Received(1).SaveAsync(default);
        Assert.True(result.IsSuccess);
    }
}
