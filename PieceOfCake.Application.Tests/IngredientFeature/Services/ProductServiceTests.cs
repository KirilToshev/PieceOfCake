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
        _productRepoMock.FirstOrDefaultAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Product, bool>>>())
            .Returns(Task.FromResult(null as Product));
        _productMock = Substitute.For<Product>();
        _productFakes = GetRequiredService<IProductFakes>();
    }

    [Fact]
    public async Task Get_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(notExistingId, CancellationToken.None)
            .Returns(Task.FromResult(null as Product));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.GetByIdAsync(notExistingId, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public async Task Get_Should_Return_Product_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(id, CancellationToken.None)
            .Returns(Task.FromResult(_productMock));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.GetByIdAsync(id, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetAll_Should_Return_Two_Products()
    {
        var carrot = _productFakes.Carrot;
        var pepper = _productFakes.Pepper;
        _productRepoMock.GetAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new Product[] { carrot, pepper } as IReadOnlyCollection<Product>));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.GetAllAsync(CancellationToken.None);

        Assert.True(result.Count == 2);
        Assert.Collection(result,
            product1 =>
            {
                Assert.Equal(carrot.Id, product1.Id);
                Assert.Equal(carrot.Name.Value, product1.Name);
            },
            product2 =>
            {
                Assert.Equal(pepper.Id, product2.Id);
                Assert.Equal(pepper.Name.Value, product2.Name);
            });
    }

    [Fact]
    public async Task Create_Should_Succseed_If_Data_Is_Valid()
    {
        //Arrange
        var createDto = Fixture.Create<ProductCreateCoreDto>();

        var sut = new ProductService(Resources, _uowMock);

        //Act
        var result = await sut.CreateAsync(createDto, CancellationToken.None);

        //Assert
        _productRepoMock.Received(1).Insert(Arg.Any<Product>());
        _uowMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        Assert.True(result.IsSuccess);
        Assert.Equal(createDto.Name, result.Value.Name);
    }

    [Fact]
    public async Task Update_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var updateDto = Fixture.Create<ProductUpdateCoreDto>();
        _productRepoMock.GetByIdAsync(updateDto.Id, CancellationToken.None)
            .Returns(Task.FromResult(null as Product));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.UpdateAsync(updateDto, CancellationToken.None);

        _productRepoMock.DidNotReceiveWithAnyArgs().Update(default);
        await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", updateDto.Id), result.Error);
    }

    [Fact]
    public async Task Update_Should_Succseed_If_Id_Is_Found ()
    {
        //Arrange
        var carrot = _productFakes.Carrot;
        var water = _productFakes.Water;
        _productMock.UpdateAsync(Arg.Is(water.Name.Value), Arg.Any<IResources>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(water));
        _productRepoMock.GetByIdAsync(Arg.Is(carrot.Id), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(carrot));
        var sut = new ProductService(Resources, _uowMock);
        var updateDto = new ProductUpdateCoreDto() { Id = carrot.Id, Name = water.Name };

        //Act
        var result = await sut.UpdateAsync(updateDto, CancellationToken.None);

        //Assert
        _productRepoMock.Received(1).Update(Arg.Any<Product>());
        await _uowMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        Assert.True(result.IsSuccess);
        Assert.Equal(water.Name, result.Value.Name);
    }

    [Fact]
    public async Task Delete_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(Arg.Is(notExistingId), CancellationToken.None)
            .Returns(Task.FromResult(null as Product));
        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.DeleteAsync(notExistingId, CancellationToken.None);

        _productRepoMock.DidNotReceiveWithAnyArgs().Delete(default);
        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }


    [Fact]
    public async Task Delete_Should_Fail_If_Product_Is_In_Use()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(id, CancellationToken.None)
            .Returns(Task.FromResult(_productMock));
        var dishMock = Substitute.For<Dish>();
        _dishRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(Task.FromResult(new Dish[] { dishMock } as IReadOnlyCollection<Dish>));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.DeleteAsync(id, CancellationToken.None);

        _productRepoMock.DidNotReceiveWithAnyArgs().Delete(default);
        Assert.True(result.IsFailure);
        Assert.Equal($"{Resources.CommonTerms.Product} can't be deleted, because it is still being used.", result.Error);
    }

    [Fact]
    public async Task Delete_Should_Succseed_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        _productRepoMock.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(_productMock));
        _dishRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(Task.FromResult(new Dish[0] as IReadOnlyCollection<Dish>));

        var sut = new ProductService(Resources, _uowMock);

        var result = await sut.DeleteAsync(id, CancellationToken.None);

        _productRepoMock.ReceivedWithAnyArgs(1).Delete(default);
        await _uowMock.ReceivedWithAnyArgs().SaveAsync(default);
        Assert.True(result.IsSuccess);
    }
}
