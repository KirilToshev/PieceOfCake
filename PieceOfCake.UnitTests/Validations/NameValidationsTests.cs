using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.Common.Validations;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Linq.Expressions;


namespace PieceOfCake.UnitTests.Validations
{
    public class NameValidationsTests
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
        public void IsUnique_Should_Return_User_Error_If_Name_Already_Exists()
        {
            var validation = new NameValidation(_uowMock.Object, _resources);

            var result = validation.IsUnique<MeasureUnit>("ads", x => x.Name == "ads");

            Assert.IsTrue(result.IsFailure);
        }
    }
}
