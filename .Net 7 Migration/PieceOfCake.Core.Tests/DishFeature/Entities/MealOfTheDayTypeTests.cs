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
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IMealOfTheDayTypeRepository> _mealTypeRepoMock;

    [SetUp]
    public void BeforeEachTest ()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _mealTypeRepoMock = new Mock<IMealOfTheDayTypeRepository>();
        _uowMock.Setup(x => x.MealOfTheDayTypeRepository)
            .Returns(_mealTypeRepoMock.Object);
        _mealTypeRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .Returns((MealOfTheDayType)null);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Create_Should_Return_User_Error_If_Created_Without_Name (string name)
    {
        var mealType = MealOfTheDayType.Create(name, Resources, _uowMock.Object);
        Assert.IsTrue(mealType.IsFailure);
        Assert.That(mealType.Error, Is.EqualTo($"{Resources.CommonTerms.MealOfTheDayType} must have name."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        var mealType = MealOfTheDayType.Create(Fixture.CreateStringOfLength(Constants.FIFTY + 1), Resources, _uowMock.Object);
        Assert.IsTrue(mealType.IsFailure);
        Assert.That(mealType.Error, Is.EqualTo($"{Resources.CommonTerms.MealOfTheDayType} name should not exceed {Constants.FIFTY} symbols."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var alreadyExistingName = Fixture.Create<string>();
        var mealType = MealOfTheDayType.Create(alreadyExistingName, Resources, _uowMock.Object).Value;
        _mealTypeRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .Returns(mealType);

        //Act
        var result = MealOfTheDayType.Create(alreadyExistingName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"An entity with name {alreadyExistingName} already exist."));
    }

    [Test]
    public void Create_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var validName = Fixture.CreateStringOfLength(Constants.FIFTY);

        //Act
        var result = MealOfTheDayType.Create(validName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(validName));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Update_Should_Return_User_Error_If_Updated_Without_Name (string updatedName)
    {
        var name = Fixture.Create<string>();
        var mealType = MealOfTheDayType.Create(name, Resources, _uowMock.Object).Value;
        _mealTypeRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .Returns(mealType);

        //Act
        var result = mealType.Update(updatedName, Resources, _uowMock.Object);

        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"{Resources.CommonTerms.MealOfTheDayType} must have name."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var mealType = MealOfTheDayType.Create(name, Resources, _uowMock.Object).Value;
        _mealTypeRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .Returns(mealType);

        //Act
        var result = mealType.Update(Fixture.CreateStringOfLength(Constants.FIFTY + 1), Resources, _uowMock.Object);

        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"{Resources.CommonTerms.MealOfTheDayType} name should not exceed {Constants.FIFTY} symbols."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var mealType = MealOfTheDayType.Create(name, Resources, _uowMock.Object).Value;
        _mealTypeRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MealOfTheDayType, bool>>>()))
            .Returns(mealType);

        //Act
        var result = mealType.Update(name, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"An entity with name {name} already exist."));
    }

    [Test]
    public void Update_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var name = Fixture.CreateStringOfLength(Constants.FIFTY);
        var measureUnit = MealOfTheDayType.Create(name, Resources, _uowMock.Object).Value;
        var updatedName = Fixture.CreateStringOfLength(Constants.TWO);

        //Act
        var result = measureUnit.Update(updatedName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(updatedName));
    }
}
