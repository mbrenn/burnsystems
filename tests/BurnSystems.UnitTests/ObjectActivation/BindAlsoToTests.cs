using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.UnitTests.ObjectActivation
{
    [TestFixture]
    public class BindAlsoToTests
    {
        [Test]
        public void TestBindByType()
        {
            var container = new ActivationContainer("TEST");
            container.Bind<TestClass>().To<TestClass>();

            var test = container.Get<ITest>();
            Ensure.IsNotNull(test);
        }

        [Test]
        public void TestBindByName()
        {
            var container = new ActivationContainer("TEST");
            container.BindToName("Name").To<TestClass>();

            var test = container.GetByName<ITest>("Name");
            Ensure.IsNotNull(test);
        }

        [Test]
        public void TestBindByTypeToConstant()
        {
            var instance = new TestClass();
            var container = new ActivationContainer("TEST");
            container.Bind<TestClass>().ToConstant(instance);

            var test = container.Get<ITest>();
            Ensure.IsNotNull(test);
        }

        public interface ITest
        {
        }

        [BindAlsoTo(typeof(ITest))]
        public class TestClass
        {
        }
    }

}
