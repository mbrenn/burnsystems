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

        [Test]
        public void TestCreationAsTransientInParent()
        {
            Helper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest", outerContainer);
            outerContainer.Bind<Helper>().To<Helper>().AsTransient();

            using (var block = new ActivationBlock("Block", innerContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.That(helper1, Is.Not.Null);
                Assert.That(helper2, Is.Not.Null);
                Assert.That(helper3, Is.Not.Null);

                Assert.AreNotSame(helper1, helper2);
                Assert.AreNotSame(helper2, helper3);
                Assert.AreNotSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(3));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(3));
        }

        [Test]
        public void TestCreationAsSingletonInParent()
        {
            Helper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest", outerContainer);
            outerContainer.Bind<Helper>().To<Helper>().AsSingleton();

            using (var block = new ActivationBlock("Block", innerContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.That(helper1, Is.Not.Null);
                Assert.That(helper2, Is.Not.Null);
                Assert.That(helper3, Is.Not.Null);

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(0));
        }

        [Test]
        public void TestCreationAsScopedInParent()
        {
            Helper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest", outerContainer);
            outerContainer.Bind<Helper>().To<Helper>().AsScoped();

            using (var block = new ActivationBlock("Block", innerContainer))
            {
                var helper1 = block.Get<Helper>();
                var helper2 = block.Get<Helper>();
                var helper3 = block.Get<Helper>();

                Assert.That(helper1, Is.Not.Null);
                Assert.That(helper2, Is.Not.Null);
                Assert.That(helper3, Is.Not.Null);

                Assert.AreSame(helper1, helper2);
                Assert.AreSame(helper2, helper3);
                Assert.AreSame(helper1, helper3);

                Assert.That(Helper.CreationCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(1));
        }

        [Test]
        public void TestParentBlockAsScoped()
        {
            Helper.Reset();
            SecondHelper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest");
            outerContainer.Bind<Helper>().To<Helper>().AsScoped();
            innerContainer.Bind<SecondHelper>().To<SecondHelper>().AsScoped();

            using (var outerBlock = new ActivationBlock("OuterBlock", outerContainer))
            {
                using (var innerBlock = new ActivationBlock("InnerBlock", innerContainer, outerBlock))
                {
                    var helpero1 = innerBlock.Get<Helper>();
                    var helpero2 = innerBlock.Get<Helper>();
                    var helpero3 = innerBlock.Get<Helper>();
                    var helperi1 = innerBlock.Get<SecondHelper>();
                    var helperi2 = innerBlock.Get<SecondHelper>();

                    Assert.That(helpero1, Is.Not.Null);
                    Assert.That(helpero2, Is.Not.Null);
                    Assert.That(helpero3, Is.Not.Null);
                    Assert.That(helperi1, Is.Not.Null);
                    Assert.That(helperi2, Is.Not.Null);

                    Assert.AreSame(helpero1, helpero2);
                    Assert.AreSame(helpero2, helpero3);
                    Assert.AreSame(helpero1, helpero3);
                    Assert.AreSame(helperi1, helperi2);
                    Assert.AreNotSame(helperi1, helpero1);

                    Assert.That(Helper.CreationCount, Is.EqualTo(1));
                    Assert.That(Helper.DisposeCount, Is.EqualTo(0));

                    Assert.That(SecondHelper.CreationCount, Is.EqualTo(1));
                    Assert.That(SecondHelper.DisposeCount, Is.EqualTo(0));
                }
                
                Assert.That(SecondHelper.DisposeCount, Is.EqualTo(1));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(1));
            Assert.That(SecondHelper.DisposeCount, Is.EqualTo(1));
        }

        [Test]
        public void TestParentBlockAsTransient()
        {
            Helper.Reset();
            SecondHelper.Reset();

            var outerContainer = new ActivationContainer("OuterTest");
            var innerContainer = new ActivationContainer("InnerTest");
            outerContainer.Bind<Helper>().To<Helper>().AsTransient();
            innerContainer.Bind<SecondHelper>().To<SecondHelper>().AsTransient();

            using (var outerBlock = new ActivationBlock("OuterBlock", outerContainer))
            {
                using (var innerBlock = new ActivationBlock("InnerBlock", innerContainer, outerBlock))
                {
                    var helpero1 = innerBlock.Get<Helper>();
                    var helpero2 = innerBlock.Get<Helper>();
                    var helpero3 = innerBlock.Get<Helper>();
                    var helperi1 = innerBlock.Get<SecondHelper>();
                    var helperi2 = innerBlock.Get<SecondHelper>();

                    Assert.That(helpero1, Is.Not.Null);
                    Assert.That(helpero2, Is.Not.Null);
                    Assert.That(helpero3, Is.Not.Null);
                    Assert.That(helperi1, Is.Not.Null);
                    Assert.That(helperi2, Is.Not.Null);

                    Assert.AreNotSame(helpero1, helpero2);
                    Assert.AreNotSame(helpero2, helpero3);
                    Assert.AreNotSame(helpero1, helpero3);
                    Assert.AreNotSame(helperi1, helperi2);
                    Assert.AreNotSame(helperi1, helpero1);

                    Assert.That(Helper.CreationCount, Is.EqualTo(3));
                    Assert.That(Helper.DisposeCount, Is.EqualTo(0));

                    Assert.That(SecondHelper.CreationCount, Is.EqualTo(2));
                    Assert.That(SecondHelper.DisposeCount, Is.EqualTo(0));
                }

                Assert.That(SecondHelper.DisposeCount, Is.EqualTo(2));
                Assert.That(Helper.DisposeCount, Is.EqualTo(0));
            }

            Assert.That(Helper.DisposeCount, Is.EqualTo(3));
            Assert.That(SecondHelper.DisposeCount, Is.EqualTo(2));
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

        /// <summary>
        /// Helper class for counting creations and disposings
        /// </summary>
        public class SecondHelper : IDisposable
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
            public SecondHelper()
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
