using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.ObjectActivation;

namespace BurnSystems.UnitTests.ObjectActivation
{
    [TestFixture]
    public class InstanceBuilderTests
    {
        [Test]
        public void TestInstanceBuilder()
        {
            var activationContainer = new ActivationContainer("Test");
			activationContainer.Bind<ICalculator>().To<Calculator>().AsTransient();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainer>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator, Is.Not.Null);
            Assert.That(container.Calculator, Is.InstanceOf<Calculator>());
        }
    }
}
