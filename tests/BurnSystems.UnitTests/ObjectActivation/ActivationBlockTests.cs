using System;
using NUnit.Framework;
using BurnSystems.ObjectActivation;

namespace BurnSystems.UnitTests.ObjectActivation
{
	[TestFixture]
	public class ActivationBlockTests
	{
        [Test]
        public void TestCreationAsSingleton()
        {
            Helper.Reset();

            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<Helper>().To<Helper>().AsSingleton();

            using (var block = new ActivationBlock("Block", activationContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(0));
        }

        [Test]
        public void TestCreationAsTransient()
        {
            Helper.Reset();

            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<Helper>().To<Helper>().AsTransient();

            using (var block = new ActivationBlock("Block", activationContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.AreNotSame(helper1, helper2);
                Assert.AreNotSame(helper2, helper3);
                Assert.AreNotSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(3));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(3));
        }

        [Test]
        public void TestCreationAsScoped()
        {
            Helper.Reset();

            var activationContainer = new ActivationContainer("Test");
            activationContainer.Bind<Helper>().To<Helper>().AsScoped();

            //////////////////
            // First activation
            using (var block = new ActivationBlock("Block", activationContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(1));

            //////////////
            // Second activation
            using (var block = new ActivationBlock("Block", activationContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(2));
                Assert.That(Helper.DisposeCount, Is.EqualTo(1));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(2));
        }

        /// <summary>
        /// Helper class for counting creations and disposings
        /// </summary>
        public class Helper : IDisposable
        {
            /// <summary>
            /// Number of creations
            /// </summary>
            public static int CreationCount
            {
                get;
                set;
            }

            /// <summary>
            /// Number of disposal
            /// </summary>
            public static int DisposeCount
            {
                get;
                set;
            }

            /// <summary>
            /// Resets disposals and creation to 0
            /// </summary>
            public static void Reset()
            {
                CreationCount = 0;
                DisposeCount = 0;
            }

            /// <summary>
            /// Constructs and increases CreationCounter
            /// </summary>
            public Helper()
            {
                CreationCount++;
            }

            /// <summary>
            /// Disposes the instance and increases Disposal Counter
            /// </summary>
            public void Dispose()
            {
                DisposeCount++;
            }
        }

	}
}
