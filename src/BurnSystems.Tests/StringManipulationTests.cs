﻿using NUnit.Framework;

namespace BurnSystems.Tests
{
    /// <summary>
    /// Contains the test concerning string manipulation
    /// </summary>
    [TestFixture]
    public class StringManipulationTests
    {
        /// <summary>
        /// Checks, if AddSlashes works as intended
        /// </summary>
        [Test]
        public void TestAddSlashes()
        {
            var testString = "c:\\Test";
            var result = StringManipulation.AddSlashes(testString);

            Assert.That(result, Is.EqualTo("c:\\\\Test"));

            testString = "c:/Test";
            result = StringManipulation.AddSlashes(testString);
            Assert.That(result, Is.EqualTo("c:/Test"));

            testString = "c:\\\\\\\\Test";
            result = StringManipulation.AddSlashes(testString);
            Assert.That(result, Is.EqualTo("c:\\\\\\\\\\\\\\\\Test"));
        }
    }
}