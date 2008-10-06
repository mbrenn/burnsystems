//-----------------------------------------------------------------------
// <copyright file="ListHelperTest.cs" company="Martin Brenn">
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
    using System.Collections.Generic;
    using BurnSystems.Collections;
    using BurnSystems.Test;

    /// <summary>
    /// Tests, if the listhelperclass is correctly implemented
    /// </summary>
    [TestClass]
    public class ListHelperTest
    {
        /// <summary>
        /// Tests, if FindMin is correctly implemented
        /// </summary>
        [TestMethod]
        public void TestFindMin()
        {
            var list = new List<double>();
            list.Add(2);
            list.Add(-4);
            list.Add(20);
            list.Add(10);

            var minimum = ListHelper.FindMin(list, x => x);

            Ensure.AreEqual(minimum, -4);
        }

        /// <summary>
        /// Tests, if FindMax is correctly implemented
        /// </summary>
        [TestMethod]
        public void TestFindMax()
        {
            var list = new List<double>();
            list.Add(2);
            list.Add(-4);
            list.Add(20);
            list.Add(10);

            var minimum = ListHelper.FindMax(list, x => x);

            Ensure.AreEqual(minimum, 20);
        }
    }
}
