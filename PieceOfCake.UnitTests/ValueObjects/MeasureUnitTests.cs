using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Linq.Expressions;

namespace PieceOfCake.UnitTests.ValueObjects
{
    public class MeasureUnitTests
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
        public void Should_Return_User_Error_If_Created_Without_Name()
        {
            var measureUnit = MeasureUnit.Create("", _resources, _uowMock.Object);
            Assert.IsTrue(measureUnit.IsFailure);
            Assert.AreEqual("Measure Unit must have name.", measureUnit.Error);
        }

        [Test]
        public void Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit()
        {
            var measureUnit = MeasureUnit.Create(new string('|', 51), _resources, _uowMock.Object);
            Assert.IsTrue(measureUnit.IsFailure);
            Assert.AreEqual("Measure Unit name should not exceed 50 symbols.", measureUnit.Error);
        }

        [Test]
        public void Should_Return_User_Error_If_Name_Already_Exists()
        {
            //Arrange
            var alreadyExistingName = _fixture.Create<string>();
            var mesureUnit = MeasureUnit.Create(alreadyExistingName, _resources, _uowMock.Object).Value;
            _measureUnitRepoMock
                .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
                .Returns(mesureUnit);

            //Act
            var measureUnit = MeasureUnit.Create(alreadyExistingName, _resources, _uowMock.Object);

            //Assert
            Assert.IsTrue(measureUnit.IsFailure);
            Assert.AreEqual(string.Format("An entity with name {0} already exist.", alreadyExistingName), measureUnit.Error);
        }
    }
}