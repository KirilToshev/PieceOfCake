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

public class MeasureUnitServiceTests : TestsBase
{
    private IUnitOfWork _uowMock;
    private IMeasureUnitRepository _measureUnitRepoMock;
    private MeasureUnit _measureUnitMock;
    private IDishRepository _dishRepoMock;
    private IMeasureUnitFakes _measureUnitFakes;

    public MeasureUnitServiceTests ()
    {
        _uowMock = Substitute.For<IUnitOfWork>();
        _measureUnitRepoMock = Substitute.For<IMeasureUnitRepository>();
        _dishRepoMock = Substitute.For<IDishRepository>();
        _uowMock.MeasureUnitRepository.Returns(_measureUnitRepoMock);
        _uowMock.DishRepository.Returns(_dishRepoMock);
        _measureUnitRepoMock.FirstOrDefaultAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<MeasureUnit, bool>>>())
            .Returns(Task.FromResult(null as MeasureUnit));
        _measureUnitMock = Substitute.For<MeasureUnit>();
        _measureUnitFakes = GetRequiredService<IMeasureUnitFakes>();
    }

    [Fact]
    public async Task Get_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _measureUnitRepoMock.GetByIdAsync(Arg.Is(notExistingId), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(null as MeasureUnit));

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = await sut.GetByIdAsync(notExistingId, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public async Task Get_Should_Return_MeasureUnit_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        var litter = _measureUnitFakes.Litter;
        _measureUnitRepoMock.GetByIdAsync(id, CancellationToken.None)
            .Returns(Task.FromResult(litter));

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = await sut.GetByIdAsync(id, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetAll_Should_Return_Two_Products()
    {
        var litter = _measureUnitFakes.Litter;
        var kg = _measureUnitFakes.Kg;
        _measureUnitRepoMock.GetAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new MeasureUnit[] { litter, kg } as IReadOnlyCollection<MeasureUnit>));

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = await sut.GetAllAsync(CancellationToken.None);

        Assert.True(result.Count == 2);
        Assert.Collection(result,
            product1 =>
            {
                Assert.Equal(litter.Id, product1.Id);
                Assert.Equal(litter.Name.Value, product1.Name);
            },
            product2 =>
            {
                Assert.Equal(kg.Id, product2.Id);
                Assert.Equal(kg.Name.Value, product2.Name);
            });
    }


    [Fact]
    public async Task Create_Should_Succseed_If_Data_Is_Valid()
    {
        //Arrange
        var createDto = Fixture.Create<MealOfTheDayTypeCreateCoreDto>();

        var sut = new MeasureUnitService(Resources, _uowMock);

        //Act
        var result = await sut.CreateAsync(createDto, CancellationToken.None);

        //Assert
        _measureUnitRepoMock.Received(1).Insert(Arg.Any<MeasureUnit>());
        _uowMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        Assert.True(result.IsSuccess);
        Assert.Equal(createDto.Name, result.Value.Name);
    }


    [Fact]
    public async Task Update_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _measureUnitRepoMock.GetByIdAsync(notExistingId, CancellationToken.None)
            .Returns(Task.FromResult(null as MeasureUnit));

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = await sut.UpdateAsync(new MeasureUnitUpdateCoreDto() { Id = notExistingId, Name = Fixture.Create<string>() }, CancellationToken.None);

        _measureUnitRepoMock.DidNotReceiveWithAnyArgs().Insert(default);
        await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
        Assert.True(result.IsFailure);
        Assert.Equal($"Element with Id={notExistingId} does not exists.", result.Error);
    }

    [Fact]
    public async Task Update_Should_Succseed_If_Id_Is_Found ()
    {
        //Arrange
        var id = Fixture.Create<Guid>();
        var updatedName = Fixture.Create<string>();
        var kg = _measureUnitFakes.Kg;
        var liter = _measureUnitFakes.Litter;
        _measureUnitMock.UpdateAsync(Arg.Is(updatedName), Arg.Any<IResources>(), Arg.Any<IUnitOfWork>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(liter));
        _measureUnitRepoMock.GetByIdAsync(Arg.Is(id), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(kg));
        var sut = new MeasureUnitService(Resources, _uowMock);

        //Act
        var result = await sut.UpdateAsync(new MeasureUnitUpdateCoreDto() { Id = id, Name = updatedName }, CancellationToken.None);

        //Assert
        _measureUnitRepoMock.Received(1).Update(Arg.Any<MeasureUnit>());
        await _uowMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Delete_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = await sut.DeleteAsync(notExistingId, CancellationToken.None);

        _measureUnitRepoMock.DidNotReceiveWithAnyArgs().Delete(default);
        await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }


    [Fact]
    public async Task Delete_Should_Fail_If_MeasureUnit_Is_In_Use()
    {
        var id = Fixture.Create<Guid>();
        _measureUnitRepoMock.GetByIdAsync(id, CancellationToken.None)
            .Returns(Task.FromResult(_measureUnitMock));
        var dishMock = Substitute.For<Dish>();
        _dishRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(Task.FromResult(new Dish[] { dishMock } as IReadOnlyCollection<Dish>));

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = await sut.DeleteAsync(id, CancellationToken.None);

        _measureUnitRepoMock.DidNotReceiveWithAnyArgs().Delete(default);
        await _uowMock.DidNotReceiveWithAnyArgs().SaveAsync(default);
        Assert.True(result.IsFailure);
        Assert.Equal($"{Resources.CommonTerms.MeasureUnit} can't be deleted, because it is still being used.", result.Error);
    }

    [Fact]
    public async Task Delete_Should_Succseed_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        _measureUnitRepoMock.GetByIdAsync(id, CancellationToken.None)
            .Returns(Task.FromResult(_measureUnitMock));
        _dishRepoMock.GetAsync(Arg.Any<CancellationToken>(), Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(Task.FromResult(new Dish[0] as IReadOnlyCollection<Dish>));

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = await sut.DeleteAsync(id, CancellationToken.None);

        _measureUnitRepoMock.Received(1).Delete(Arg.Any<MeasureUnit>());
        await _uowMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        Assert.True(result.IsSuccess);
    }
}
