using AutoFixture;
using CSharpFunctionalExtensions;
using NSubstitute;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Application.IngredientFeature.Services;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Application.Tests.IngredientFeature.Services;

public class ProductServiceTests : TestsBase
{
    private IUnitOfWork _uowMock;
    private IProductRepository _productRepoMock;
    private IDishRepository _dishRepoMock;
    private Product _productMock;
    private IProductFakes _productFakes;

    public ProductServiceTests()
    {
        _uowMock = Substitute.For<IUnitOfWork>();
        _productRepoMock = Substitute.For<IProductRepository>();
        _dishRepoMock = Substitute.For<IDishRepository>();
        _uowMock.ProductRepository
            .Returns(_productRepoMock);
        _uowMock.DishRepository
            .Returns(_dishRepoMock);
        _productRepoMock.FirstOrDefaultAsync(Arg.Any<Expression<Func<Product, bool>>>())
            .Returns(Task.FromResult(null as Product));
        _productMock = Substitute.For<Product>();
        _productFakes = GetRequiredService<IProductFakes>();
    }

    [Fact]
    public async Task Get_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(notExistingId)
            .Returns(Task.FromResult(null as Product));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.GetByIdAsync(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public async Task Get_Should_Return_MeasureUnit_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(id)
            .Returns(Task.FromResult(_productMock));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.GetByIdAsync(id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Update_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var updateDto = Fixture.Create<ProductUpdateDto>();
        _productRepoMock.GetByIdAsync(updateDto.Id)
            .Returns(Task.FromResult(null as Product));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.UpdateAsync(updateDto);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", updateDto.Id), result.Error);
    }

    [Fact]
    public async Task Update_Should_Succseed_If_Id_Is_Found ()
    {
        //Arrange
        var id = Fixture.Create<Guid>();
        var updatedName = Fixture.Create<string>();
        var carrot = _productFakes.Carrot;
        var water = _productFakes.Water;
        _productMock.UpdateAsync(Arg.Is(updatedName), Arg.Any<IResources>(), Arg.Any<IUnitOfWork>())
            .Returns(Result.Success(water));
        _productRepoMock.GetByIdAsync(Arg.Is(id))
            .Returns(Task.FromResult(carrot));
        var sut = new ProductService(Resources, _uowMock);

        //Act
        var result = await sut.UpdateAsync(new ProductUpdateDto() { Id = id, Name = updatedName });

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Delete_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(Arg.Is(notExistingId))
            .Returns(Task.FromResult(null as Product));
        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.DeleteAsync(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public async Task Delete_Should_Succseed_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(Arg.Is(id))
            .Returns(Task.FromResult(_productMock));
        _dishRepoMock.GetAsync(Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(Task.FromResult(new Dish[0] as IReadOnlyCollection<Dish>));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.DeleteAsync(id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_Should_Fail_If_Product_Is_In_Use ()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(id)
            .Returns(Task.FromResult(_productMock));
        var dishMock = Substitute.For<Dish>();
        _dishRepoMock.GetAsync(Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(Task.FromResult(new Dish[] { dishMock } as IReadOnlyCollection<Dish>));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.DeleteAsync(id);

        Assert.True(result.IsFailure);
        Assert.Equal($"{Resources.CommonTerms.Product} can't be deleted, because it is still being used.", result.Error);
    }
}
