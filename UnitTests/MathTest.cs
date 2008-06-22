//-----------------------------------------------------------------------
// <copyright file="MathTest.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.UnitTests
{
    using BurnSystems.Test;

    /// <summary>
    /// A simple test class for Mathtest
    /// </summary>
    [TestClass]
    public class MathTest
    {
        /// <summary>
        /// Tests the MathHelper.Max-Method
        /// </summary>
        [TestMethod]
        public void TestMax()
        {
            var o = new int[] { 2, 5, 7 };

            Ensure.AreEqual(MathHelper.Max(o), 7);
        }

        /// <summary>
        /// Tests the MathHelper.Min-Method
        /// </summary>
        [TestMethod]
        public void TestMin()
        {
            var o = new int[] { 2, 5, 7 };

            Ensure.AreEqual(MathHelper.Min(o), 2);
        }
    }
}
