using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.ObjectActivation;
using BurnSystems.UnitTests.ObjectActivation.Objects;

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

        [Test]
        public void TestInstanceEmbedded()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<CalculationContainer>().To<CalculationContainer>();
            activationContainer.Bind<ICalculator>().To<Calculator>();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainerContainer>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Container, Is.Not.Null);
            Assert.That(container.Container.Calculator, Is.Not.Null);
            Assert.That(container, Is.TypeOf<CalculationContainerContainer>());
            Assert.That(container.Container, Is.TypeOf<CalculationContainer>());
            Assert.That(container.Container.Calculator, Is.TypeOf<Calculator>());
        }
        [Test]
        public void TestInstanceOnlyGetter()
        {
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>();

            var instanceBuilder = new InstanceBuilder(activationContainer);
            var container = instanceBuilder.Create<CalculationContainerGetter>();
            Assert.That(container, Is.Not.Null);
            Assert.That(container.Calculator, Is.Null);
        }
    }
}
