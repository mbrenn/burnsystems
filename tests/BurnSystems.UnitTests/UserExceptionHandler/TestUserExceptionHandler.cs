using BurnSystems.UserExceptionHandler;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.UnitTests.UserExceptionHandler
{
    [TestFixture]
    public class TestUserExceptionHandler
    {
        [Test]
        public void TestNullExceptionHandlers()
        {
            var handler = new StandardExceptionHandling(null);
            Assert.Throws<NotHandledException>(() => handler.HandleException(new Exception()));
        }

        [Test]
        public void TestEmptyExceptionHandlers()
        {
            var handler = new StandardExceptionHandling(new IUserExceptionHandler[] { });
            Assert.Throws<NotHandledException>(() => handler.HandleException(new Exception()));
        }

        [Test]
        public void TestTrueExceptionHandlers()
        {
            var handler = new StandardExceptionHandling(new IUserExceptionHandler[] { new TrueExceptionHandler() });
            done = false;
            handler.HandleException(new Exception());
            Assert.That(done, Is.EqualTo(true));
        }

        [Test]
        public void TestFalseExceptionHandlers()
        {
            var handler = new StandardExceptionHandling(new IUserExceptionHandler[] { new FalseExceptionHandler() });
            done = false;
            Assert.Throws<NotHandledException>(() => handler.HandleException(new Exception()));
            Assert.That(done, Is.EqualTo(true));
        }

        public static bool done = false;

        public class TrueExceptionHandler : IUserExceptionHandler
        {
            public bool Handle(Exception exc)
            {
                done = true;
                return true;
            }
        }

        public class FalseExceptionHandler : IUserExceptionHandler
        {
            public bool Handle(Exception exc)
            {
                done = true;
                return false;
            }
        }

    }
}
