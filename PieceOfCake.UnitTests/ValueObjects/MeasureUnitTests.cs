using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;

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
        }

        [Test]
        public void Should_Return_User_Error_If_Created_Without_Name()
        {
            var measureUnit = MeasureUnit.Create("", _resources);
            Assert.IsTrue(measureUnit.IsFailure);
            Assert.AreEqual("Measure Unit must have name.", measureUnit.Error);
        }
    }
}