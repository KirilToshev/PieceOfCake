using AutoFixture;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.IngredientFeature.Entities;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Tests.IngredientFeature.Entities;

public class MeasureUnitTests : TestsBase
{
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IMeasureUnitRepository> _measureUnitRepoMock;

    [SetUp]
    public void BeforeEachTest ()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _measureUnitRepoMock = new Mock<IMeasureUnitRepository>();
        _uowMock.Setup(x => x.MeasureUnitRepository)
            .Returns(_measureUnitRepoMock.Object);
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns((MeasureUnit)null);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Create_Should_Return_User_Error_If_Created_Without_Name (string measureUnitName)
    {
        var measureUnit = MeasureUnit.Create(measureUnitName, Resources, _uowMock.Object);
        Assert.IsTrue(measureUnit.IsFailure);
        Assert.That(measureUnit.Error, Is.EqualTo("Measure Unit must have name."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        var measureUnit = MeasureUnit.Create(new string('|', 51), Resources, _uowMock.Object);
        Assert.IsTrue(measureUnit.IsFailure);
        Assert.That(measureUnit.Error, Is.EqualTo("Measure Unit name should not exceed 50 symbols."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var alreadyExistingName = Fixture.Create<string>();
        var measureUnit = MeasureUnit.Create(alreadyExistingName, Resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns(measureUnit);

        //Act
        var result = MeasureUnit.Create(alreadyExistingName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"An entity with name {alreadyExistingName} already exist."));
    }

    [Test]
    public void Create_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var validName = new string('|', 50);
        
        //Act
        var result = MeasureUnit.Create(validName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(validName));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Update_Should_Return_User_Error_If_Updated_Without_Name (string measureUnitName)
    {
        var name = Fixture.Create<string>();
        var measureUnit = MeasureUnit.Create(name, Resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns(measureUnit);

        //Act
        var result = measureUnit.Update(measureUnitName, Resources, _uowMock.Object);

        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo("Measure Unit must have name."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var measureUnit = MeasureUnit.Create(name, Resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns(measureUnit);

        //Act
        var result = measureUnit.Update(new string('|', 51), Resources, _uowMock.Object);

        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo("Measure Unit name should not exceed 50 symbols."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var measureUnit = MeasureUnit.Create(name, Resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns(measureUnit);

        //Act
        var result = measureUnit.Update(name, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"An entity with name {name} already exist."));
    }

    [Test]
    public void Update_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var name = new string('|', 50);
        var measureUnit = MeasureUnit.Create(name, Resources, _uowMock.Object).Value;
        var updatedName = new string('|', 1);

        //Act
        var result = measureUnit.Update(updatedName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(updatedName));
    }
}
