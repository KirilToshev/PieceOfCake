using AutoFixture;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Tests.Common;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Tests.DishFeature.Entities;

public class MealOfTheDayTypeTests : TestsBase
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IMealOfTheDayTypeRepository> _mealTypeRepoMock;

    public MealOfTheDayTypeTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _mealTypeRepoMock = new Mock<IMealOfTheDayTypeRepository>();
    }

    [SetUp]
    public async Task BeforeEachTest ()
    {
        _uowMock.Setup(x => x.MealOfTheDayTypeRepository)
            .Returns(_mealTypeRepoMock.Object);
        _mealTypeRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .ReturnsAsync(null as MealOfTheDayType);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public async Task Create_Should_Return_User_Error_If_Created_Without_Name (string? name)
    {
        var mealType = await MealOfTheDayType.Create(name, Resources, _uowMock.Object, CancellationToken.None);
        Assert.That(mealType.IsFailure);
        Assert.That(mealType.Error, Is.EqualTo($"{Resources.CommonTerms.MealOfTheDayType} must have name."));
    }

    [Test]
    public async Task Create_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        var mealType = await MealOfTheDayType.Create(Fixture.CreateStringOfLength(Constants.FIFTY + 1), Resources, _uowMock.Object, CancellationToken.None);
        Assert.That(mealType.IsFailure);
        Assert.That(mealType.Error, Is.EqualTo($"{Resources.CommonTerms.MealOfTheDayType} name should not exceed {Constants.FIFTY} symbols."));
    }

    [Test]
    public async Task Create_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var alreadyExistingName = Fixture.Create<string>();
        var mealType = await MealOfTheDayType.Create(alreadyExistingName, Resources, _uowMock.Object, CancellationToken.None);
        _mealTypeRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .ReturnsAsync(mealType.Value);

        //Act
        var result = await MealOfTheDayType.Create(alreadyExistingName, Resources, _uowMock.Object, CancellationToken.None);

        //Assert
        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"An entity with name {alreadyExistingName} already exist."));
    }

    [Test]
    public async Task Create_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var validName = Fixture.CreateStringOfLength(Constants.FIFTY);

        //Act
        var result = await MealOfTheDayType.Create(validName, Resources, _uowMock.Object, CancellationToken.None);

        //Assert
        Assert.That(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(validName));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public async Task Update_Should_Return_User_Error_If_Updated_Without_Name (string? updatedName)
    {
        var name = Fixture.Create<string>();
        var mealType = await MealOfTheDayType.Create(name, Resources, _uowMock.Object, CancellationToken.None);
        _mealTypeRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .ReturnsAsync(mealType.Value);

        //Act
        var result = await mealType.Value.UpdateAsync(updatedName, Resources, _uowMock.Object, CancellationToken.None);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"{Resources.CommonTerms.MealOfTheDayType} must have name."));
    }

    [Test]
    public async Task Update_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var mealType = await MealOfTheDayType.Create(name, Resources, _uowMock.Object, CancellationToken.None);
        _mealTypeRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .ReturnsAsync(mealType.Value);

        //Act
        var result = await mealType.Value.UpdateAsync(Fixture.CreateStringOfLength(Constants.FIFTY + 1), Resources, _uowMock.Object, CancellationToken.None);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"{Resources.CommonTerms.MealOfTheDayType} name should not exceed {Constants.FIFTY} symbols."));
    }

    [Test]
    public async Task Update_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var mealType = await MealOfTheDayType.Create(name, Resources, _uowMock.Object, CancellationToken.None);
        _mealTypeRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .ReturnsAsync(mealType.Value);

        //Act
        var result = await mealType.Value.UpdateAsync(name, Resources, _uowMock.Object, CancellationToken.None);

        //Assert
        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"An entity with name {name} already exist."));
    }

    [Test]
    public async Task Update_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var name = Fixture.CreateStringOfLength(Constants.FIFTY);
        var measureUnit = await MealOfTheDayType.Create(name, Resources, _uowMock.Object, CancellationToken.None);
        var updatedName = Fixture.CreateStringOfLength(Constants.TWO);

        //Act
        var result = await measureUnit.Value.UpdateAsync(updatedName, Resources, _uowMock.Object, CancellationToken.None);

        //Assert
        Assert.That(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(updatedName));
    }
}
