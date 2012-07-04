﻿using System;
using BurnSystems.ObjectActivation;
using NUnit.Framework;

namespace BurnSystems.UnitTests.ObjectActivation
{
	[TestFixture]
	public class ActivationContainerTests
	{
		[Test]
		public void TestTypeCreation()
		{
			var activationContainer = new ActivationContainer("Test");
			activationContainer.Bind<ICalculator>().To<Calculator>().AsTransient();
			
			var calculator = activationContainer.Get<ICalculator>();
			var type = calculator.GetType();
			Assert.That(calculator, Is.TypeOf(typeof(Calculator)));

			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(5));
		}
		
		[Test]
		public void TestConstant()
		{
			var activationContainer = new ActivationContainer("Test");
			var myCalculator = new Calculator();
			myCalculator.InternalAddOffset = 5;
			activationContainer.Bind<ICalculator>().ToConstant(myCalculator).AsTransient();
			
			var calculator = activationContainer.Get<ICalculator>();			
			Assert.That(calculator, Is.TypeOf(typeof(Calculator)));

			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(10));
		}
		
		[Test]
		public void TestFactory()
		{
			var activationContainer = new ActivationContainer("Test");
			var myCalculator = new Calculator();
			activationContainer.Bind<ICalculator>().To(
				() => new Calculator()
				{
					InternalAddOffset = 7
				}).AsTransient();
			
			var calculator = activationContainer.Get<ICalculator>();			
			Assert.That(calculator, Is.TypeOf(typeof(Calculator)));

			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(12));
		}
		
		[Test]
		public void TestTypeCreationAsSingleton()
		{
			// Initial creation
			var activationContainer = new ActivationContainer("Test");
			activationContainer.Bind<ICalculator>().To<Calculator>().AsSingleton();
			
			var calculator = activationContainer.Get<ICalculator>();
			var type = calculator.GetType();
			Assert.That(calculator, Is.TypeOf(typeof(Calculator)));

			// First get
			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(5));		
			
			// Change singleton
			((Calculator) calculator).InternalAddOffset = 10;
			
			result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(15));		
			
			// Now, retrieve the variable again, we'd like to have
			// the same instance! 
			calculator = activationContainer.Get<ICalculator>();
			result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(15));					
		}

        [Test]
        public void TestTypeCreationAsScoped()
        {
            // Initial creation
            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<ICalculator>().To<Calculator>().AsScoped();

            Assert.Throws<InvalidOperationException>(() => activationContainer.Get<ICalculator>());
        }

		[Test]
		public void TestTransientRetrievalFromParent()
		{
			var outerContainer = new ActivationContainer("Outer");
			var innerContainer = new ActivationContainer("Inner", outerContainer);
			
			outerContainer.Bind<ICalculator>().To<Calculator>();

			var calculator = innerContainer.Get<ICalculator>();
			Assert.That(calculator, Is.Not.Null);
			
			var result = calculator.Add(2,3);
			Assert.That(result, Is.EqualTo(5));
		}

		[Test]
		public void TestTransientRetrievalFromParentOverride()
		{
			var outerContainer = new ActivationContainer("Outer");
			var innerContainer = new ActivationContainer("Inner", outerContainer);
			
			outerContainer.Bind<ICalculator>().To<Calculator>();
			innerContainer.Bind<ICalculator>().To(
				() => new Calculator()
				{
					InternalAddOffset = 5
				});

			// Inner Calculator is Adder with offset = 5
			var innerCalculator = innerContainer.Get<ICalculator>();
			Assert.That(innerCalculator, Is.Not.Null);
			
			var result = innerCalculator.Add(2,3);
			Assert.That(result, Is.EqualTo(10));
			
			// Outer Calculator is Adder with offset = 0
			var outerCalculator = outerContainer.Get<ICalculator>();
			Assert.That(outerCalculator, Is.Not.Null);
			
			var result2 = outerCalculator.Add(2,3);
			Assert.That(result2, Is.EqualTo(5));
		}
		
		[Test]
		public void TestSingletonRetrievalFromParentOverride()
		{
			var outerContainer = new ActivationContainer("Outer");
			var innerContainer = new ActivationContainer("Inner", outerContainer);
			
			outerContainer.Bind<ICalculator>().To<Calculator>().AsSingleton();
			innerContainer.Bind<ICalculator>().To(
				() => new Calculator()
				{
					InternalAddOffset = 5
				})
				.AsSingleton();
			
			var innerCalculator = innerContainer.Get<ICalculator>();
			var outerCalculator = outerContainer.Get<ICalculator>();

			var innerResult = innerCalculator.Add(5,3);
			var outerResult = outerCalculator.Add(5,3);
			
			Assert.That(innerResult, Is.EqualTo(13));
			Assert.That(outerResult, Is.EqualTo(8));
			
			// Change behaviour of instance
			((Calculator)innerCalculator).InternalAddOffset = 10;			
			
			innerCalculator = innerContainer.Get<ICalculator>();
			outerCalculator = outerContainer.Get<ICalculator>();

			innerResult = innerCalculator.Add(5,3);
			outerResult = outerCalculator.Add(5,3);
			
			Assert.That(innerResult, Is.EqualTo(18));
			Assert.That(outerResult, Is.EqualTo(8));

		}

        public object Exceptionvar { get; set; }
    }
}
