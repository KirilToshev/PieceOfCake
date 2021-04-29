using AutoFixture;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.DomainServices;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System;
using System.Linq.Expressions;

namespace PieceOfCake.UnitTests.Core.DomainServices
{
    public class MeasureUnitDomainServiceTests
    {
        private IResources _resources;
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMeasureUnitRepository> _measureUnitRepoMock;
        private Fixture _fixture;
        private Mock<MeasureUnit> _measureUnitMock;
        private Mock<IDishRepository> _dishRepoMock;

        [SetUp]
        public void BeforeEachTest()
        {
            _fixture = new Fixture();
            IServiceCollection services = new ServiceCollection();
            services.AddResources();
            var serviceProvider = services.BuildServiceProvider();
            _resources = serviceProvider.GetService<IResources>();
            _uowMock = new Mock<IUnitOfWork>();
            _measureUnitRepoMock = new Mock<IMeasureUnitRepository>();
            _dishRepoMock = new Mock<IDishRepository>();
            _uowMock.Setup(x => x.MeasureUnitRepository)
                .Returns(_measureUnitRepoMock.Object);
            _uowMock.Setup(x => x.DishRepository)
                .Returns(_dishRepoMock.Object);
            _measureUnitRepoMock
                .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
                .Returns((MeasureUnit)null);
            _measureUnitMock = new Mock<MeasureUnit>();
        }

        [Test]
        public void Get_Should_Return_User_Error_If_Id_Is_Not_Found()
        {
            var notExistingId = _fixture.Create<long>();
            _measureUnitRepoMock
                .Setup(x => x.GetById(notExistingId))
                .Returns((MeasureUnit)null);

            var sut = new MeasureUnitDomainService(_resources, _uowMock.Object);

            var result = sut.Get(notExistingId);

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
        }

        [Test]
        public void Get_Should_Return_MeasureUnit_If_Id_Is_Found()
        {
            var id = _fixture.Create<long>();
            _measureUnitRepoMock
                .Setup(x => x.GetById(id))
                .Returns(_measureUnitMock.Object);

            var sut = new MeasureUnitDomainService(_resources, _uowMock.Object);

            var result = sut.Get(id);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void Update_Should_Return_User_Error_If_Id_Is_Not_Found()
        {
            var notExistingId = _fixture.Create<long>();
            _measureUnitRepoMock
                .Setup(x => x.GetById(notExistingId))
                .Returns((MeasureUnit)null);

            var sut = new MeasureUnitDomainService(_resources, _uowMock.Object);

            var result = sut.Update(notExistingId, _fixture.Create<string>());

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
        }

        [Test]
        public void Update_Should_Succseed_If_Id_Is_Found()
        {
            //Arrange
            var id = _fixture.Create<long>();
            var updatedName = _fixture.Create<string>();
            _measureUnitMock.Setup(x => x.Update(updatedName, It.IsAny<IResources>(), It.IsAny<IUnitOfWork>()))
                .Returns(Result.Success(_measureUnitMock.Object));
            _measureUnitRepoMock
                .Setup(x => x.GetById(id))
                .Returns(_measureUnitMock.Object);
            var sut = new MeasureUnitDomainService(_resources, _uowMock.Object);

            //Act
            var result = sut.Update(id, updatedName);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void Delete_Should_Return_User_Error_If_Id_Is_Not_Found()
        {
            var notExistingId = _fixture.Create<long>();
            var sut = new MeasureUnitDomainService(_resources, _uowMock.Object);

            var result = sut.Delete(notExistingId);

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
        }

        [Test]
        public void Delete_Should_Succseed_If_Id_Is_Found()
        {
            var id = _fixture.Create<long>();
            _measureUnitRepoMock
                .Setup(x => x.GetById(id))
                .Returns(_measureUnitMock.Object);
            _dishRepoMock
                .Setup(x => x.Get(It.IsAny<Expression<Func<Dish, bool>>>(), null))
                .Returns(new Dish[0]);

            var sut = new MeasureUnitDomainService(_resources, _uowMock.Object);

            var result = sut.Delete(id);

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void Delete_Should_Fail_If_MeasureUnit_Is_In_Use()
        {
            var id = _fixture.Create<long>();
            _measureUnitRepoMock
                .Setup(x => x.GetById(id))
                .Returns(_measureUnitMock.Object);
            var dishMock = new Mock<Dish>();
            _dishRepoMock
                .Setup(x => x.Get(It.IsAny<Expression<Func<Dish, bool>>>(), null))
                .Returns(new Dish[] { dishMock.Object });

            var sut = new MeasureUnitDomainService(_resources, _uowMock.Object);

            var result = sut.Delete(id);

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual($"{_resources.CommonTerms.MeasureUnit} can't be deleted, because it is still being used.", result.Error);
        }
    }
}