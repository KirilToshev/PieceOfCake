using AutoFixture;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.DomainServices;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Linq.Expressions;

namespace PieceOfCake.UnitTests.Core.DomainServices
{
    public class ProductDomainServiceTests
    {
        private IResources _resources;
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IProductRepository> _productRepoMock;
        private Fixture _fixture;
        private Mock<Product> _productMock;

        [SetUp]
        public void BeforeEachTest()
        {
            _fixture = new Fixture();
            IServiceCollection services = new ServiceCollection();
            services.AddResources();
            var serviceProvider = services.BuildServiceProvider();
            _resources = serviceProvider.GetService<IResources>();
            _uowMock = new Mock<IUnitOfWork>();
            _productRepoMock = new Mock<IProductRepository>();
            _uowMock.Setup(x => x.ProductRepository)
                .Returns(_productRepoMock.Object);
            _productRepoMock
                .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns((Product)null);
            _productMock = new Mock<Product>();
        }

        [Test]
        public void Get_Should_Return_User_Error_If_Id_Is_Not_Found()
        {
            var notExistingId = _fixture.Create<long>();
            _productRepoMock
                .Setup(x => x.GetById(notExistingId))
                .Returns((Product)null);

            var sut = new ProductDomainService(_resources, _uowMock.Object);

            var result = sut.Get(notExistingId);

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
        }

        [Test]
        public void Get_Should_Return_MeasureUnit_If_Id_Is_Found()
        {
            var id = _fixture.Create<long>();
            _productRepoMock
                .Setup(x => x.GetById(id))
                .Returns(_productMock.Object);

            var sut = new ProductDomainService(_resources, _uowMock.Object);

            var result = sut.Get(id);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void Update_Should_Return_User_Error_If_Id_Is_Not_Found()
        {
            var notExistingId = _fixture.Create<long>();
            _productRepoMock
                .Setup(x => x.GetById(notExistingId))
                .Returns((Product)null);

            var sut = new ProductDomainService(_resources, _uowMock.Object);

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
            _productMock.Setup(x => x.Update(updatedName, It.IsAny<IResources>(), It.IsAny<IUnitOfWork>()))
                .Returns(Result.Ok(_productMock.Object));
            _productRepoMock
                .Setup(x => x.GetById(id))
                .Returns(_productMock.Object);
            var sut = new ProductDomainService(_resources, _uowMock.Object);

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
            var sut = new ProductDomainService(_resources, _uowMock.Object);

            var result = sut.Delete(notExistingId);

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
        }

        [Test]
        public void Delete_Should_Succseed_If_Id_Is_Found()
        {
            var id = _fixture.Create<long>();
            _productRepoMock
                .Setup(x => x.GetById(id))
                .Returns(_productMock.Object);

            var sut = new ProductDomainService(_resources, _uowMock.Object);

            var result = sut.Delete(id);

            Assert.IsTrue(result.IsSuccess);
        }
    }
}