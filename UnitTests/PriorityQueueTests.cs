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
    using System;
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

            var test = new PriorityQueue<object>();
            test.Add(23);
            test.Add(25);
            test.Add(10);

            Ensure.AreEqual(test.Count, 3);
            Ensure.AreEqual(test.Pop(), 25);
            Ensure.AreEqual(test.Pop(), 23);
            Ensure.AreEqual(test.Pop(), 10);
            Ensure.IsNull(test.Pop());

            test.Add(23);
            test.Add(25);
            test.Add(10);
            Ensure.AreEqual(test.GetFirst(), 25);

            test.Remove(10);
            Ensure.AreEqual(test.GetFirst(), 25);
            Ensure.AreEqual(test.Pop(), 25);
            Ensure.AreEqual(test.Pop(), 23);
            Ensure.AreEqual(test.Count, 0);

            test.Add(23);
            test.Add(25);
            test.Add(10);

            test.Remove(25);
            Ensure.AreEqual(test.GetFirst(), 23);
            Ensure.AreEqual(test.Pop(), 23);
            Ensure.AreEqual(test.Pop(), 10);
            
            test.Add(23);
            test.Add(23);
            test.Add(23);
            test.Add(25);
            test.Add(23);
            test.Add(25);
            test.Add(25);
            test.Add(10);
            test.Add(25);
            test.Add(10);
            
            Ensure.AreEqual(test.Pop(), 25);
            Ensure.AreEqual(test.Pop(), 25);
            Ensure.AreEqual(test.Pop(), 25);
            Ensure.AreEqual(test.Pop(), 25);
            Ensure.AreEqual(test.Pop(), 23);
            Ensure.AreEqual(test.Pop(), 23);
            Ensure.AreEqual(test.Pop(), 23);
            Ensure.AreEqual(test.Pop(), 23);
            Ensure.AreEqual(test.Pop(), 10);
            Ensure.AreEqual(test.Pop(), 10);

            for (var n = 0; n < 10000; n++)
            {
                priorityQueue.Add(MathHelper.Random.Next());
            }

            var last = Int32.MaxValue;
            while (test.Count > 0)
            {
                var current = priorityQueue.Pop();
                Ensure.IsSmallerOrEqual(current, last);
                current = last;
            }
        }
    }
}
