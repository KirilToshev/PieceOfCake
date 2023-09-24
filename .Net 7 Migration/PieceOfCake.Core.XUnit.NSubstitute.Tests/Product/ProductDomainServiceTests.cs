using AutoFixture;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PieceOfCake.Application.Product.Services;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System.Linq.Expressions;

namespace PieceOfCake.Application.Tests.Product;

public class ProductDomainServiceTests
{
    private IResources _resources;
    private IUnitOfWork _uowMock;
    private IProductRepository _productRepoMock;
    private IDishRepository _dishRepoMock;
    private Fixture _fixture;
    private Core.Entities.Product _productMock;

    public ProductDomainServiceTests ()
    {
        _fixture = new Fixture();
        IServiceCollection services = new ServiceCollection();
        services.AddResources();
        var serviceProvider = services.BuildServiceProvider();
        _resources = serviceProvider.GetService<IResources>();
        _uowMock = Substitute.For<IUnitOfWork>();
        _productRepoMock = Substitute.For<IProductRepository>();
        _dishRepoMock = Substitute.For<IDishRepository>();
        _uowMock.ProductRepository
            .Returns(_productRepoMock);
        _uowMock.DishRepository
            .Returns(_dishRepoMock);
        _productRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<Core.Entities.Product, bool>>>())
            .Returns((Core.Entities.Product)null);
        _productMock = Substitute.For<Core.Entities.Product>();
    }

    [Fact]
    public void Get_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = _fixture.Create<Guid>();
        _productRepoMock.GetById(notExistingId)
            .Returns((Core.Entities.Product)null);

        var sut = new ProductService(_resources, _uowMock);

        var result = sut.Get(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public void Get_Should_Return_MeasureUnit_If_Id_Is_Found ()
    {
        var id = _fixture.Create<Guid>();
        _productRepoMock.GetById(id)
            .Returns(_productMock);

        var sut = new ProductService(_resources, _uowMock);

        var result = sut.Get(id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void Update_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = _fixture.Create<Guid>();
        _productRepoMock.GetById(notExistingId)
            .Returns((Core.Entities.Product)null);

        var sut = new ProductService(_resources, _uowMock);

        var result = sut.Update(notExistingId, _fixture.Create<string>());

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public void Update_Should_Succseed_If_Id_Is_Found ()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var updatedName = _fixture.Create<string>();
        _productMock.Update(updatedName, Arg.Any<IResources>(), Arg.Any<IUnitOfWork>())
            .Returns(Result.Success(_productMock));
        _productRepoMock.GetById(id)
            .Returns(_productMock);
        var sut = new ProductService(_resources, _uowMock);

        //Act
        var result = sut.Update(id, updatedName);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void Delete_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = _fixture.Create<Guid>();
        _productRepoMock.GetById(Arg.Is(notExistingId)).Returns(x => null);
        var sut = new ProductService(_resources, _uowMock);

        var result = sut.Delete(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public void Delete_Should_Succseed_If_Id_Is_Found ()
    {
        var id = _fixture.Create<Guid>();
        _productRepoMock.GetById(Arg.Is(id))
            .Returns(_productMock);
        _dishRepoMock.Get(Arg.Any<Expression<Func<Core.Entities.Dish, bool>>>(), null)
            .Returns(new Core.Entities.Dish[0]);

        var sut = new ProductService(_resources, _uowMock);

        var result = sut.Delete(id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Delete_Should_Fail_If_Product_Is_In_Use ()
    {
        var id = _fixture.Create<Guid>();
        _productRepoMock.GetById(id)
            .Returns(_productMock);
        var dishMock = Substitute.For<Core.Entities.Dish>();
        _dishRepoMock.Get(Arg.Any<Expression<Func<Core.Entities.Dish, bool>>>(), null)
            .Returns(new Core.Entities.Dish[] { dishMock });

        var sut = new ProductService(_resources, _uowMock);

        var result = sut.Delete(id);

        Assert.True(result.IsFailure);
        Assert.Equal($"{_resources.CommonTerms.Product} can't be deleted, because it is still being used.", result.Error);
    }
}
