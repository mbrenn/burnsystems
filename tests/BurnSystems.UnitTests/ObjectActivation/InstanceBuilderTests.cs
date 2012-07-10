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

        [Test]
        public void TestInstanceBuilderByName()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>().AsTransient();
            activationContainer.BindToName("Add").To<Calculator>().AsTransient();
            activationContainer.BindToName("AddByTwo").To<CalculatorAddByTwo>().AsTransient();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainerByName>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.CalculatorByType, Is.Not.Null);
            Assert.That(container.Calculator, Is.Not.Null);
            Assert.That(container.CalculatorByTwo, Is.Not.Null);
            Assert.That(container.Calculator, Is.InstanceOf<Calculator>());
            Assert.That(container.CalculatorByType, Is.InstanceOf<Calculator>());
            Assert.That(container.CalculatorByTwo, Is.InstanceOf<CalculatorAddByTwo>());
        }
    }
}
