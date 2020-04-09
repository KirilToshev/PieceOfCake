using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PieceOfCake.BusinessRules;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Linq.Expressions;

namespace PieceOfCake.UnitTests.BusinessRules
{
    public class MeasureUnitBusinessRulesTests
    {
        private IResources _resources;
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMeasureUnitRepository> _measureUnitRepoMock;
        private Fixture _fixture; 

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
            _uowMock.Setup(x => x.MeasureUnitRepository)
                .Returns(_measureUnitRepoMock.Object);
            _measureUnitRepoMock
                .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
                .Returns((MeasureUnit)null);
        }

        [Test]
        public void Get_Should_Return_User_Error_If_Id_Is_Not_Found()
        {
            var notExistingId = _fixture.Create<long>();
            _measureUnitRepoMock
                .Setup(x => x.GetById(notExistingId))
                .Returns((MeasureUnit)null);

            var sut = new MeasureUnitBr(_resources, _uowMock.Object);

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
                .Returns(MeasureUnit.Create(_fixture.Create<string>(), _resources, _uowMock.Object).Value);

            var sut = new MeasureUnitBr(_resources, _uowMock.Object);

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

            var sut = new MeasureUnitBr(_resources, _uowMock.Object);

            var result = sut.Update(notExistingId, _fixture.Create<string>());

            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(string.Format("Element with Id={0} does not exists.", notExistingId), result.Error);
        }

        [Test]
        public void Update_Should_Succseed_If_Id_Is_Found()
        {
            var id = _fixture.Create<long>();
            _measureUnitRepoMock
                .Setup(x => x.GetById(id))
                .Returns(MeasureUnit.Create(_fixture.Create<string>(), _resources, _uowMock.Object).Value);

            var sut = new MeasureUnitBr(_resources, _uowMock.Object);

            var updatedName = _fixture.Create<string>();
            var result = sut.Update(id, updatedName);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(updatedName, (string)result.Value.Name);
        }

        [Test]
        public void Delete_Should_Return_User_Error_If_Id_Is_Not_Found()
        {
            var notExistingId = _fixture.Create<long>();
            var sut = new MeasureUnitBr(_resources, _uowMock.Object);

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
                .Returns(MeasureUnit.Create(_fixture.Create<string>(), _resources, _uowMock.Object).Value);

            var sut = new MeasureUnitBr(_resources, _uowMock.Object);

            var result = sut.Delete(id);

            Assert.IsTrue(result.IsSuccess);
        }
    }
}