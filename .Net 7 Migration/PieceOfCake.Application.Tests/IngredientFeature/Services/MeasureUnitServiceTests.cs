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
        _measureUnitRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<MeasureUnit, bool>>>()).Returns((MeasureUnit)null);
        _measureUnitMock = Substitute.For<MeasureUnit>();
        _measureUnitFakes = GetRequiredService<IMeasureUnitFakes>();
    }

    [Fact]
    public async Task Get_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(Arg.Is(notExistingId))
            .Returns((MeasureUnit)null);

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = sut.GetByIdAsync(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public async Task Get_Should_Return_MeasureUnit_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        var test = _measureUnitFakes.Litter;
        _measureUnitRepoMock.GetById(id).Returns(test);

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = sut.GetByIdAsync(id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Update_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(notExistingId).Returns((MeasureUnit)null);

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = sut.UpdateAsync(new MeasureUnitUpdateDto() { Id = notExistingId, Name = Fixture.Create<string>() });

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public async Task Update_Should_Succseed_If_Id_Is_Found ()
    {
        //Arrange
        var id = Fixture.Create<Guid>();
        var updatedName = Fixture.Create<string>();
        var kg = _measureUnitFakes.Kg;
        var liter = _measureUnitFakes.Litter;
        _measureUnitMock.Update(Arg.Is(updatedName), Arg.Any<IResources>(), Arg.Any<IUnitOfWork>())
            .Returns(Result.Success(liter));
        _measureUnitRepoMock.GetById(Arg.Is(id))
            .Returns(kg);
        var sut = new MeasureUnitService(Resources, _uowMock);

        //Act
        var result = sut.UpdateAsync(new MeasureUnitUpdateDto() { Id = id, Name = updatedName });

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Delete_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = Fixture.Create<Guid>();
        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = sut.DeleteAsync(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public async Task Delete_Should_Succseed_If_Id_Is_Found ()
    {
        var id = Fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(id)
            .Returns(_measureUnitMock);
        _dishRepoMock.GetAsync(Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(new Dish[0]);

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = sut.DeleteAsync(id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_Should_Fail_If_MeasureUnit_Is_In_Use ()
    {
        var id = Fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(id)
            .Returns(_measureUnitMock);
        var dishMock = Substitute.For<Dish>();
        _dishRepoMock.GetAsync(Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(new Dish[] { dishMock });

        var sut = new MeasureUnitService(Resources, _uowMock);

        var result = sut.DeleteAsync(id);

        Assert.True(result.IsFailure);
        Assert.Equal($"{Resources.CommonTerms.MeasureUnit} can't be deleted, because it is still being used.", result.Error);
    }
}
