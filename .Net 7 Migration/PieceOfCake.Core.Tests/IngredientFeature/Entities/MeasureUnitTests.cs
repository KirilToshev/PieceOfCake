using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.IngredientFeature.Entities;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Tests.IngredientFeature.Entities;

public class MeasureUnitTests
{
    private IResources _resources;
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IMeasureUnitRepository> _measureUnitRepoMock;
    private Fixture _fixture;

    [SetUp]
    public void BeforeEachTest ()
    {
        _fixture = new Fixture();
        IServiceCollection services = new ServiceCollection();
        services.AddResources();
        var serviceProvider = services.BuildServiceProvider();
        _resources = serviceProvider.GetService<IResources>();
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
        var measureUnit = MeasureUnit.Create(measureUnitName, _resources, _uowMock.Object);
        Assert.IsTrue(measureUnit.IsFailure);
        Assert.AreEqual("Measure Unit must have name.", measureUnit.Error);
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        var measureUnit = MeasureUnit.Create(new string('|', 51), _resources, _uowMock.Object);
        Assert.IsTrue(measureUnit.IsFailure);
        Assert.AreEqual("Measure Unit name should not exceed 50 symbols.", measureUnit.Error);
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var alreadyExistingName = _fixture.Create<string>();
        var measureUnit = MeasureUnit.Create(alreadyExistingName, _resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns(measureUnit);

        //Act
        var result = MeasureUnit.Create(alreadyExistingName, _resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(string.Format("An entity with name {0} already exist.", alreadyExistingName), result.Error);
    }

    [Test]
    public void Create_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var validName = new string('|', 50);
        var measureUnit = MeasureUnit.Create(validName, _resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns((MeasureUnit)null);

        //Act
        var result = MeasureUnit.Create(validName, _resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Update_Should_Return_User_Error_If_Created_Without_Name (string measureUnitName)
    {
        var name = _fixture.Create<string>();
        var measureUnit = MeasureUnit.Create(name, _resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns(measureUnit);

        //Act
        var result = measureUnit.Update(measureUnitName, _resources, _uowMock.Object);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual("Measure Unit must have name.", result.Error);
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        //Arrange
        var name = _fixture.Create<string>();
        var measureUnit = MeasureUnit.Create(name, _resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns(measureUnit);

        //Act
        var result = measureUnit.Update(new string('|', 51), _resources, _uowMock.Object);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual("Measure Unit name should not exceed 50 symbols.", result.Error);
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var name = _fixture.Create<string>();
        var measureUnit = MeasureUnit.Create(name, _resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns(measureUnit);

        //Act
        var result = measureUnit.Update(name, _resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(string.Format("An entity with name {0} already exist.", name), result.Error);
    }

    [Test]
    public void Update_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var name = new string('|', 50);
        var measureUnit = MeasureUnit.Create(name, _resources, _uowMock.Object).Value;
        _measureUnitRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
            .Returns((MeasureUnit)null);

        //Act
        var result = measureUnit.Update(new string('|', 1), _resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
