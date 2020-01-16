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
        
        [SetUp]
        public void BeforeEachTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddResources();
            var serviceProvider = services.BuildServiceProvider();
            _resources = serviceProvider.GetService<IResources>();
            var moq = new Mock<IMeasureUnitRepository>()
                .Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<MeasureUnit, bool>>>()))
                .Returns(x => )
                
        }

        [Test]
        public void Should_Return_User_Error_If_Created_Without_Name()
        {
            var measureUnit = MeasureUnit.Create("", _resources);
            Assert.IsTrue(measureUnit.IsFailure);
            Assert.AreEqual("Measure Unit must have name.", measureUnit.Error);
        }

        [Test]
        public void Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit()
        {
            var measureUnit = MeasureUnit.Create(new string('|', 51), _resources);
            Assert.IsTrue(measureUnit.IsFailure);
            Assert.AreEqual("Measure Unit name should not exceed 50 symbols.", measureUnit.Error);
        }

        [Test]
        public void Should_Return_User_Error_If_Name_Exceeds_Symbols_Count_Limit()
        {
            var measureUnit = MeasureUnit.Create(new string('|', 51), _resources);
            Assert.IsTrue(measureUnit.IsFailure);
            Assert.AreEqual("Measure Unit name should not exceed 50 symbols.", measureUnit.Error);
        }
    }
}