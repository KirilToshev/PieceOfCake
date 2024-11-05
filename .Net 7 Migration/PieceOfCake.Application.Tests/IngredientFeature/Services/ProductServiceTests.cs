using AutoFixture;
using CSharpFunctionalExtensions;
using NSubstitute;
using PieceOfCake.Application.IngredientFeature.Services;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;
using System.Linq.Expressions;

namespace PieceOfCake.Application.Tests.IngredientFeature.Services;

public class ProductServiceTests : TestsBase
{
    private IUnitOfWork _uowMock;
    private IProductRepository _productRepoMock;
    private IDishRepository _dishRepoMock;
    private Product _productMock;

    public ProductServiceTests()
    {
        _uowMock = Substitute.For<IUnitOfWork>();
        _productRepoMock = Substitute.For<IProductRepository>();
        _dishRepoMock = Substitute.For<IDishRepository>();
        _uowMock.ProductRepository
            .Returns(_productRepoMock);
        _uowMock.DishRepository
            .Returns(_dishRepoMock);
        _productRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Product, bool>>>())
            .Returns((Product)null);
        _productMock = Substitute.For<Product>();
    }

    [Fact]
    public void Get_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _productRepoMock.GetById(notExistingId)
            .Returns((Product)null);

        var sut = new ProductService(Resources, _uowMock);

        var result = sut.Get(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public void Get_Should_Return_MeasureUnit_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetById(id)
            .Returns(_productMock);

        var sut = new ProductService(Resources, _uowMock);

        var result = sut.Get(id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void Update_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _productRepoMock.GetById(notExistingId)
            .Returns((Product)null);

        var sut = new ProductService(Resources, _uowMock);

        var result = sut.Update(notExistingId, Fixture.Create<string>());

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public void Update_Should_Succseed_If_Id_Is_Found ()
    {
        //Arrange
        var id = Fixture.Create<Guid>();
        var updatedName = Fixture.Create<string>();
        _productMock.Update(updatedName, Arg.Any<IResources>(), Arg.Any<IUnitOfWork>())
            .Returns(Result.Success(_productMock));
        _productRepoMock.GetById(id)
            .Returns(_productMock);
        var sut = new ProductService(Resources, _uowMock);

        //Act
        var result = sut.Update(id, updatedName);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void Delete_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _productRepoMock.GetById(Arg.Is(notExistingId)).Returns(x => null);
        var sut = new ProductService(Resources, _uowMock);

        var result = sut.Delete(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public void Delete_Should_Succseed_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetById(Arg.Is(id))
            .Returns(_productMock);
        _dishRepoMock.GetAsync(Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(new Dish[0]);

        var sut = new ProductService(Resources, _uowMock);

        var result = sut.Delete(id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Delete_Should_Fail_If_Product_Is_In_Use ()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetById(id)
            .Returns(_productMock);
        var dishMock = Substitute.For<Dish>();
        _dishRepoMock.GetAsync(Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(new Dish[] { dishMock });

        var sut = new ProductService(Resources, _uowMock);

        var result = sut.Delete(id);

        Assert.True(result.IsFailure);
        Assert.Equal($"{Resources.CommonTerms.Product} can't be deleted, because it is still being used.", result.Error);
    }
}
