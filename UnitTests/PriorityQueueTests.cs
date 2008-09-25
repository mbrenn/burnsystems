//-----------------------------------------------------------------------
// <copyright file="PriorityQueue.cs" company="Martin Brenn">
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
    using BurnSystems.Collections;
    using BurnSystems.Test;

    /// <summary>
    /// Test classes
    /// </summary>
    [TestClass]    
    public class PriorityQueueTests
    {
        /// <summary>
        /// Tests the priority queue
        /// </summary>
        [TestMethod]
        public void TestPriorityQueue()
        {
            var priorityQueue = new PriorityQueue<int>();
        }
    }
}
