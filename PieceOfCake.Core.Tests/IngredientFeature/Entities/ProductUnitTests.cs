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

    public ProductUnitTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _productRepoMock = new Mock<IProductRepository>();
        _productMock = new Mock<Product>();
        _nameMock = new Mock<Name>();
    }

    [SetUp]
    public async Task BeforeEachTest ()
    {
        _uowMock.Setup(x => x.ProductRepository)
            .Returns(_productRepoMock.Object);
        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(null as Product);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public async Task Create_Should_Return_User_Error_If_Created_Without_Name (string? productName)
    {
        var productResult = await Product.CreateAsync(productName, Resources, _uowMock.Object, CancellationToken.None);
        Assert.That(productResult.IsFailure);
        Assert.That(productResult.Error, Is.EqualTo("Product must have name."));
    }

    [Test]
    public async Task Create_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        var productResult = await Product.CreateAsync(Fixture.CreateStringOfLength(Constants.FIFTY + 1), Resources, _uowMock.Object, CancellationToken.None);
        Assert.That(productResult.IsFailure);
        Assert.That(productResult.Error, Is.EqualTo($"{Resources.CommonTerms.Product} name should not exceed {Constants.FIFTY} symbols."));
    }

    [Test]
    public async Task Create_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var alreadyExistingName = Fixture.Create<string>();

        _nameMock.SetupGet(x => x.Value)
            .Returns(alreadyExistingName);
        _productMock
            .SetupGet(x => x.Name)
            .Returns(_nameMock.Object);

        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(_productMock.Object);

        //Act
        var productResult = await Product.CreateAsync(alreadyExistingName, Resources, _uowMock.Object, CancellationToken.None);

        //Assert
        Assert.That(productResult.IsFailure);
        Assert.That(productResult.Error, Is.EqualTo($"An entity with name {alreadyExistingName} already exist."));
    }

    [Test]
    public async Task Create_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var validName = Fixture.CreateStringOfLength(Constants.FIFTY);
        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(null as Product);

        //Act
        var result = await Product.CreateAsync(validName, Resources, _uowMock.Object, CancellationToken.None);

        //Assert
        Assert.That(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(validName));
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public async Task Update_Should_Return_User_Error_If_Created_Without_Name (string? productName)
    {
        var product = await Product.CreateAsync(Fixture.Create<string>(), Resources, _uowMock.Object, CancellationToken.None);
        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(product.Value);

        //Act
        var result = await product.Value.UpdateAsync(productName, Resources, _uowMock.Object, CancellationToken.None);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"{Resources.CommonTerms.Product} must have name."));
    }

    [Test]
    public async Task Update_Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit ()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var product = await Product.CreateAsync(name, Resources, _uowMock.Object, CancellationToken.None);
        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(product.Value);

        //Act
        var result = await product.Value.UpdateAsync(Fixture.CreateStringOfLength(Constants.FIFTY + 1), Resources, _uowMock.Object, CancellationToken.None);

        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"{Resources.CommonTerms.Product} name should not exceed {Constants.FIFTY} symbols."));
    }

    [Test]
    public async Task Update_Should_Return_User_Error_If_Name_Already_Exists ()
    {
        //Arrange
        var product = await Product
            .CreateAsync(Fixture.Create<string>(), Resources, _uowMock.Object, CancellationToken.None);
        var alreadyExistingName = Fixture.Create<string>();
        _nameMock.SetupGet(x => x.Value)
            .Returns(alreadyExistingName);
        _productMock
            .SetupGet(x => x.Name)
            .Returns(_nameMock.Object);
        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(_productMock.Object);

        //Act
        var result = await product.Value.UpdateAsync(alreadyExistingName, Resources, _uowMock.Object, CancellationToken.None);

        //Assert
        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo($"An entity with name {alreadyExistingName} already exist."));
    }

    [Test]
    public async Task Update_Should_Succseed_If_Name_Meets_Requirenements ()
    {
        //Arrange
        var name = Fixture.CreateStringOfLength(Constants.FIFTY);
        var product = await Product.CreateAsync(name, Resources, _uowMock.Object, CancellationToken.None);
        _productRepoMock
            .Setup(x => x.FirstOrDefaultAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(null as Product);

        //Act
        var updatedName = Fixture.CreateStringOfLength(Constants.TWO);
        var result = await product.Value.UpdateAsync(updatedName, Resources, _uowMock.Object, CancellationToken.None);

        //Assert
        Assert.That(result.IsSuccess);
        Assert.That(result.Value.Name.Value, Is.EqualTo(updatedName));
    }
}
