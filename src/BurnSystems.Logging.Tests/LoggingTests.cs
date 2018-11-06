using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using BurnSystems.Logging.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BurnSystems.Logging.Tests
{
    [TestClass]
    public class LoggingTests
    {
        /// <summary>
        /// Stores the logger for the tests
        /// </summary>
        private readonly ClassLogger _logger = new ClassLogger(typeof(LoggingTests));

        [TestMethod]
        public void TestFramework()
        {
            var inMemoryProvider = new InMemoryDatabaseProvider();
            TheLog.AddProvider(inMemoryProvider, LogLevel.Trace);

            TheLog.Info("Test");
            TheLog.Trace("Test2");
            TheLog.Error("Test");
            
            Assert.AreEqual(3, inMemoryProvider.Messages.Count);

            inMemoryProvider.ClearLog();
            Assert.AreEqual(0, inMemoryProvider.Messages.Count);

            TheLog.Log(LogLevel.Info, "Message");
            Assert.AreEqual(1, inMemoryProvider.Messages.Count);
        }

        [TestMethod]
        public void TestFiltering()
        {
            var inMemoryProvider = new InMemoryDatabaseProvider();
            TheLog.AddProvider(inMemoryProvider, LogLevel.Info);

            TheLog.Info("Test");
            TheLog.Trace("Test2");
            TheLog.Error("Test");

            Assert.AreEqual(2, inMemoryProvider.Messages.Count);
        }

        [TestMethod]
        public void TestFilteringOfLog()
        {
            var inMemoryProvider = new InMemoryDatabaseProvider();
            TheLog.AddProvider(inMemoryProvider, LogLevel.Trace);
            TheLog.FilterThreshold = LogLevel.Info;
            Assert.AreEqual(LogLevel.Info, TheLog.FilterThreshold);

            TheLog.Info("Test");
            TheLog.Trace("Test2");
            TheLog.Error("Test");

            Assert.AreEqual(2, inMemoryProvider.Messages.Count);
        }

        [TestMethod]
        public void TestClassLogger()
        {
            var inMemoryProvider = new InMemoryDatabaseProvider();
            TheLog.AddProvider(inMemoryProvider, LogLevel.Info);
            TheLog.FilterThreshold = LogLevel.Info;

            _logger.Info("Test");
            _logger.Trace("Test2");
            _logger.Error("Test");

            Assert.AreEqual(2, inMemoryProvider.Messages.Count);
            Assert.IsTrue(inMemoryProvider.Messages[0].LogMessage.Category.Contains("LoggingTests"));

            inMemoryProvider.ClearLog();
            _logger.Error("Test", "ABC");
            Assert.IsTrue(inMemoryProvider.Messages[0].LogMessage.Category.Contains("LoggingTests"));
            Assert.IsTrue(inMemoryProvider.Messages[0].LogMessage.Category.Contains("ABC"));
        }

        [TestMethod]
        public void TestConsoleLogger()
        {
            TheLog.AddProvider(new ConsoleProvider(), LogLevel.Info);
            _logger.Info("Info");   // will be shown
            _logger.Trace("Trace"); // will be ignored
            _logger.Error("Error"); // will be shown
        }

        [TestMethod]
        public void TestLogLevel()
        {
            var database = new InMemoryDatabaseProvider();
            TheLog.FilterThreshold = LogLevel.Trace;
            TheLog.AddProvider(database, LogLevel.Trace);

            TheLog.Trace("Test");
            Assert.AreEqual(LogLevel.Trace, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            TheLog.Debug("Test");
            Assert.AreEqual(LogLevel.Debug, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            TheLog.Info("Test");
            Assert.AreEqual(LogLevel.Info, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            TheLog.Warn("Test");
            Assert.AreEqual(LogLevel.Warn, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            TheLog.Error("Test");
            Assert.AreEqual(LogLevel.Error, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            TheLog.Fatal("Test");
            Assert.AreEqual(LogLevel.Fatal, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
        }


        [TestMethod]
        public void TestClassLoggerLevel()
        {
            TheLog.FilterThreshold = LogLevel.Trace;
            TheLog.AddProvider(InMemoryDatabaseProvider.TheOne, LogLevel.Trace);
            var database = InMemoryDatabaseProvider.TheOne;

            var classLogger = new ClassLogger(typeof(LoggingTests));
            classLogger.Trace("Test");
            Assert.AreEqual(LogLevel.Trace, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            classLogger.Debug("Test");
            Assert.AreEqual(LogLevel.Debug, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            classLogger.Info("Test");
            Assert.AreEqual(LogLevel.Info, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            classLogger.Warn("Test");
            Assert.AreEqual(LogLevel.Warn, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            classLogger.Error("Test");
            Assert.AreEqual(LogLevel.Error, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
            classLogger.Fatal("Test");
            Assert.AreEqual(LogLevel.Fatal, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);

            classLogger.Log(LogLevel.Debug, "Message");
            Assert.AreEqual(LogLevel.Debug, database.Messages[database.Messages.Count - 1].LogMessage.LogLevel);
        }

        [TestMethod]
        public void TestFileLogging()
        {
            using (var fileProvider = new FileProvider("./test.txt", true))
            {
                TheLog.AddProvider(fileProvider, LogLevel.Info);
                _logger.Info("Info");   // will be shown
                _logger.Trace("Trace"); // will be ignored
                _logger.Error("Error"); // will be shown
            }

            var lines = File.ReadAllLines("./test.txt");
            Assert.AreEqual(2, lines.Length);
            Assert.IsTrue(lines[0].Contains("Info"));
            Assert.IsTrue(lines[1].Contains("Error"));
            TheLog.ClearProviders();

            using (var fileProvider = new FileProvider("./test.txt", false))
            {
                TheLog.AddProvider(fileProvider, LogLevel.Info);
                _logger.Info("Info");   // will be shown
                _logger.Trace("Trace"); // will be ignored
                _logger.Error("Error"); // will be shown
            }

            lines = File.ReadAllLines("./test.txt");
            Assert.AreEqual(4, lines.Length);
            TheLog.ClearProviders();

            using (var fileProvider = new FileProvider("./test.txt", true))
            {
                TheLog.AddProvider(fileProvider, LogLevel.Info);
                _logger.Info("Info");   // will be shown
                _logger.Trace("Trace"); // will be ignored
                _logger.Error("Error"); // will be shown
            }

            lines = File.ReadAllLines("./test.txt");
            Assert.AreEqual(2, lines.Length);
            TheLog.ClearProviders();
        }

        [TestMethod]
        public void TestDebug()
        {
            TheLog.FilterThreshold = LogLevel.Trace;
            TheLog.AddProvider(new DebugProvider(), LogLevel.Trace);
            _logger.Info("Yeah");
        }

        [TestMethod]
        public void TestToString()
        {
            var logMessage = new LogMessage()
            {
                LogLevel = LogLevel.Trace,
                Message = "ABC"
            };

            var x = logMessage.ToString();
            logMessage.Category = "DEF";

            var y = logMessage.ToString();

            Assert.IsTrue(x.Contains("Trace"));
            Assert.IsTrue(x.Contains("ABC"));
            Assert.IsTrue(y.Contains("Trace"));
            Assert.IsTrue(y.Contains("ABC"));
            Assert.IsTrue(y.Contains("DEF"));
        }

        [TestMethod]
        public void TestEventProvider()
        {
            var x = 0;
            var eventHandler = new EventProvider();
            eventHandler.MessageReceived += (a, b) =>
            {
                x++;
                Assert.AreEqual(LogLevel.Info, b.LogMessage.LogLevel);
            };

            TheLog.FilterThreshold = LogLevel.Trace;
            TheLog.AddProvider(eventHandler, LogLevel.Trace);

            Assert.AreEqual(0, x);
            _logger.Info("Yeah");

            Assert.AreEqual(1, x);
        }
    }
}
