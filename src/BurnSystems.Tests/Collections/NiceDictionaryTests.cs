using BurnSystems.Collections;
using NUnit.Framework;

namespace BurnSystems.Tests.Collections
{
    /// <summary>
    /// Implements some tests for the nice dictionary
    /// </summary>
    [TestFixture]
    public class NiceDictionaryTests
    {
        [Test]
        public void TestEntries()
        {
            var niceDictionary = new NiceDictionary<int, int>
            {
                [2] = 4, 
                [5] = 25
            };

            Assert.That(niceDictionary[2], Is.EqualTo(4));
            Assert.That(niceDictionary[5], Is.EqualTo(25));
            Assert.That(niceDictionary[3], Is.EqualTo(0));

            var niceDictionary2 = new NiceDictionary<string, string>
            {
                ["four"] = "4", 
                ["five"] = "5"
            };

            Assert.That(niceDictionary2["four"], Is.EqualTo("4"));
            Assert.That(niceDictionary2["five"], Is.EqualTo("5"));
            Assert.That(niceDictionary2["six"], Is.Null);
        }
    }
}
