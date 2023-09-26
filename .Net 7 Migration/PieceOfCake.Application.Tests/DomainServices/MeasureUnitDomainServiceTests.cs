using AutoFixture;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PieceOfCake.Application.Services;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Entities;
using System.Linq.Expressions;

namespace PieceOfCake.Application.Tests.DomainServices;

public class MeasureUnitDomainServiceTests
{
    private IResources _resources;
    private IUnitOfWork _uowMock;
    private IMeasureUnitRepository _measureUnitRepoMock;
    private Fixture _fixture;
    private MeasureUnit _measureUnitMock;
    private IDishRepository _dishRepoMock;

    public MeasureUnitDomainServiceTests ()
    {
        _fixture = new Fixture();
        IServiceCollection services = new ServiceCollection();
        services.AddResources();
        var serviceProvider = services.BuildServiceProvider();
        _resources = serviceProvider.GetService<IResources>();

        _uowMock = Substitute.For<IUnitOfWork>();
        _measureUnitRepoMock = Substitute.For<IMeasureUnitRepository>();
        _dishRepoMock = Substitute.For<IDishRepository>();
        _uowMock.MeasureUnitRepository.Returns(_measureUnitRepoMock);
        _uowMock.DishRepository.Returns(_dishRepoMock);
        _measureUnitRepoMock.GetFirstOrDefault(Arg.Any<Expression<Func<MeasureUnit, bool>>>()).Returns((MeasureUnit)null);
        _measureUnitMock = Substitute.For<MeasureUnit>();
    }

    [Fact]
    public void Get_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = _fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(Arg.Is(notExistingId))
            .Returns((MeasureUnit)null);

        var sut = new MeasureUnitService(_resources, _uowMock);

        var result = sut.Get(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public void Get_Should_Return_MeasureUnit_If_Id_Is_Found ()
    {
        var id = _fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(id).Returns(_measureUnitMock);

        var sut = new MeasureUnitService(_resources, _uowMock);

        var result = sut.Get(id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void Update_Should_Return_User_Error_If_Id_Is_Not_Found ()
    {
        var notExistingId = _fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(notExistingId).Returns((MeasureUnit)null);

        var sut = new MeasureUnitService(_resources, _uowMock);

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
        _measureUnitMock.Update(Arg.Is(updatedName), Arg.Any<IResources>(), Arg.Any<IUnitOfWork>())
            .Returns(Result.Success(_measureUnitMock));
        _measureUnitRepoMock.GetById(Arg.Is(id))
            .Returns(_measureUnitMock);
        var sut = new MeasureUnitService(_resources, _uowMock);

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
        var sut = new MeasureUnitService(_resources, _uowMock);

        var result = sut.Delete(notExistingId);

        Assert.True(result.IsFailure);
        Assert.Equal(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
    }

    [Fact]
    public void Delete_Should_Succseed_If_Id_Is_Found ()
    {
        var id = _fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(id)
            .Returns(_measureUnitMock);
        _dishRepoMock.Get(Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(new Dish[0]);

        var sut = new MeasureUnitService(_resources, _uowMock);

        var result = sut.Delete(id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Delete_Should_Fail_If_MeasureUnit_Is_In_Use ()
    {
        var id = _fixture.Create<Guid>();
        _measureUnitRepoMock.GetById(id)
            .Returns(_measureUnitMock);
        var dishMock = Substitute.For<Dish>();
        _dishRepoMock.Get(Arg.Any<Expression<Func<Dish, bool>>>(), null)
            .Returns(new Dish[] { dishMock });

        var sut = new MeasureUnitService(_resources, _uowMock);

        var result = sut.Delete(id);

        Assert.True(result.IsFailure);
        Assert.Equal($"{_resources.CommonTerms.MeasureUnit} can't be deleted, because it is still being used.", result.Error);
    }
}
