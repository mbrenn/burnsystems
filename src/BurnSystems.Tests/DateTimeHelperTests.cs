using System;
using NUnit.Framework;

namespace BurnSystems.Tests
{
    /// <summary>
    /// Test class for DateTimeHelper
    /// </summary>
    [TestFixture]
    public class DateTimeHelperTests
    {
        [Test]
        public void TestGoBackToHour()
        {
            var time = new DateTime(8, 9, 10, 23, 10, 10); // 10.09.08 23:10:10
            var timeBack10 = time.GoBackToHour(10);
            var timeBack23 = time.GoBackToHour(23);

            Assert.That(timeBack10, Is.EqualTo(new DateTime(8, 9, 10, 10, 0, 0)));
            Assert.That(timeBack23, Is.EqualTo(new DateTime(8, 9, 10, 23, 0, 0)));

            time = new DateTime(8, 9, 10, 11, 10, 10); // 10.09.08 23:10:10
            timeBack10 = time.GoBackToHour(10);
            timeBack23 = time.GoBackToHour(23);

            Assert.That(timeBack10, Is.EqualTo(new DateTime(8, 9, 10, 10, 0, 0)));
            Assert.That(timeBack23, Is.EqualTo(new DateTime(8, 9, 09, 23, 0, 0)));
        }

        [Test]
        public void TestTruncation()
        {
            var time = new DateTime(2012, 11, 16, 11, 42, 25, 543);

            var toQuarter = DateTimeHelper.Truncate(time, TimeSpan.FromMinutes(15));
            var toMinute = DateTimeHelper.TruncateToMinute(time);
            var toSecond = DateTimeHelper.TruncateToSecond(time);

            Assert.That(toQuarter, Is.EqualTo(new DateTime(2012, 11, 16, 11, 30, 0, 0)));
            Assert.That(toMinute, Is.EqualTo(new DateTime(2012, 11, 16, 11, 42, 0, 0)));
            Assert.That(toSecond, Is.EqualTo(new DateTime(2012, 11, 16, 11, 42, 25, 0)));

            Assert.That(DateTimeHelper.Truncate(DateTime.MinValue, TimeSpan.FromSeconds(10)), Is.EqualTo(DateTime.MinValue));
            Assert.That(DateTimeHelper.Truncate(DateTime.MaxValue, TimeSpan.FromSeconds(10)), Is.EqualTo(DateTime.MaxValue));
            
            Assert.That(DateTimeHelper.Truncate(time, TimeSpan.Zero), Is.EqualTo(time));
        }
    }
}