using AutoFixture;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.ValueObjects;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Tests.Common;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Tests.IngredientFeature.Entities;

public class ProductUnitTests : TestsBase
{
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IProductRepository> _productRepoMock;
    private Mock<Product> _productMock;
    private Mock<Name> _nameMock;

    [SetUp]
    public void BeforeEachTest ()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _productRepoMock = new Mock<IProductRepository>();
        _uowMock.Setup(x => x.ProductRepository)
            .Returns(_productRepoMock.Object);
        _productRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns((Product)null);
        _productMock = new Mock<Product>();
        _nameMock = new Mock<Name>();
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Create_Should_Return_User_Error_If_Created_Without_Name (string productName)
    {
        var productResult = Product.Create(productName, Resources, _uowMock.Object);
        Assert.IsTrue(productResult.IsFailure);
        Assert.That(productResult.Error, Is.EqualTo("Product must have name."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        var productResult = Product.Create(Fixture.CreateStringOfLength(Constants.FIFTY + 1), Resources, _uowMock.Object);
        Assert.IsTrue(productResult.IsFailure);
        Assert.That(productResult.Error, Is.EqualTo($"{Resources.CommonTerms.Product} name should not exceed {Constants.FIFTY} symbols."));
    }

    [Test]
    public void Create_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var alreadyExistingName = Fixture.Create<string>();

        _nameMock.SetupGet(x => x.Value)
            .Returns(alreadyExistingName);
        _productMock
            .SetupGet(x => x.Name)
            .Returns(_nameMock.Object);

        _productRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(_productMock.Object);

        //Act
        var productResult = Product.Create(alreadyExistingName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(productResult.IsFailure);
        Assert.That(productResult.Error, Is.EqualTo($"An entity with name {alreadyExistingName} already exist."));
    }

    [Test]
    public void Create_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var validName = Fixture.CreateStringOfLength(Constants.FIFTY);
        _productRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns((Product)null);

        //Act
        var result = Product.Create(validName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(validName));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void Update_Should_Return_User_Error_If_Created_Without_Name (string productName)
    {
        var product = Product.Create(Fixture.Create<string>(), Resources, _uowMock.Object).Value;
        _productRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(product);

        //Act
        var result = product.Update(productName, Resources, _uowMock.Object);

        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"{Resources.CommonTerms.Product} must have name."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var product = Product.Create(name, Resources, _uowMock.Object).Value;
        _productRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(product);

        //Act
        var result = product.Update(Fixture.CreateStringOfLength(Constants.FIFTY + 1), Resources, _uowMock.Object);

        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"{Resources.CommonTerms.Product} name should not exceed {Constants.FIFTY} symbols."));
    }

    [Test]
    public void Update_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var product = Product
            .Create(Fixture.Create<string>(), Resources, _uowMock.Object).Value;
        var alreadyExistingName = Fixture.Create<string>();
        _nameMock.SetupGet(x => x.Value)
            .Returns(alreadyExistingName);
        _productMock
            .SetupGet(x => x.Name)
            .Returns(_nameMock.Object);
        _productRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(_productMock.Object);

        //Act
        var result = product.Update(alreadyExistingName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"An entity with name {alreadyExistingName} already exist."));
    }

    [Test]
    public void Update_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var name = Fixture.CreateStringOfLength(Constants.FIFTY);
        var product = Product.Create(name, Resources, _uowMock.Object).Value;
        _productRepoMock
            .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns((Product)null);

        //Act
        var updatedName = Fixture.CreateStringOfLength(Constants.TWO);
        var result = product.Update(updatedName, Resources, _uowMock.Object);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(updatedName));
    }
}
