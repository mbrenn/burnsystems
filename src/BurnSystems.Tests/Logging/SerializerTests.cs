using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BurnSystems.Logging;
using BurnSystems.Logging.Pipe;
using NUnit.Framework;

namespace BurnSystems.Tests.Logging
{
    [TestFixture]
    public class SerializerTests
    {
        [Test]
        public async Task TestConversionOfLogMessage()
        {
            // First message
            var logMessage = new LogMessage
            {
                Category = "This is a test",
                Message = "Yes",
                LogLevel = LogLevel.Debug
            };

            var bytes = LogMessageSerializer.ConvertMessage(logMessage);
            
            Assert.That(bytes, Is.Not.Null);
            
            var byteStream = new MemoryStream(bytes);

            var messageBack = await LogMessageSerializer.ParseMessage(byteStream);
            Assert.That(messageBack != null, Is.True);
            Debug.Assert(messageBack != null, nameof(messageBack) + " != null");
            
            Assert.That(messageBack.Category, Is.EqualTo(logMessage.Category));
            Assert.That(messageBack.Message, Is.EqualTo(logMessage.Message));
            Assert.That(messageBack.LogLevel, Is.EqualTo(logMessage.LogLevel));
            
            // Second message
            logMessage = new LogMessage
            {
                Category = "This is a test",
                Message = "Yes",
                LogLevel = LogLevel.Fatal
            };
            
            bytes = LogMessageSerializer.ConvertMessage(logMessage);
            
            Assert.That(bytes, Is.Not.Null);
            
            byteStream = new MemoryStream(bytes);

            messageBack = await LogMessageSerializer.ParseMessage(byteStream);
            Assert.That(messageBack != null, Is.True);
            Debug.Assert(messageBack != null, nameof(messageBack) + " != null");
            
            Assert.That(messageBack.Category, Is.EqualTo(logMessage.Category));
            Assert.That(messageBack.Message, Is.EqualTo(logMessage.Message));
            Assert.That(messageBack.LogLevel, Is.EqualTo(logMessage.LogLevel));
        }
        
        [Test]
        public async Task TestConversionOfLogMetricMessageInteger()
        {
            // First message
            var logMessage = new LogMetricMessage<int>
            {
                Category = "This is a test",
                Message = "Yes",
                LogLevel = LogLevel.Debug,
                Value = 4323,
                Unit = "m"
            };

            var bytes = LogMessageSerializer.ConvertMessage(logMessage);
            
            Assert.That(bytes, Is.Not.Null);
            
            var byteStream = new MemoryStream(bytes);

            var messageBack = await LogMessageSerializer.ParseMessage(byteStream) as LogMetricMessage<int>;
            Assert.That(messageBack != null, Is.True);
            Debug.Assert(messageBack != null, nameof(messageBack) + " != null");
            
            Assert.That(messageBack.Category, Is.EqualTo(logMessage.Category));
            Assert.That(messageBack.Message, Is.EqualTo(logMessage.Message));
            Assert.That(messageBack.LogLevel, Is.EqualTo(logMessage.LogLevel));
            Assert.That(messageBack.Value, Is.EqualTo(logMessage.Value));
            Assert.That(messageBack.Unit, Is.EqualTo(logMessage.Unit));
            
            // Second message
            logMessage = new LogMetricMessage<int>
            {
                Category = "This is a test",
                Message = "Yes",
                LogLevel = LogLevel.Fatal,
                Value = -323,
                Unit = "km"
            };
            
            bytes = LogMessageSerializer.ConvertMessage(logMessage);
            
            Assert.That(bytes, Is.Not.Null);
            
            byteStream = new MemoryStream(bytes);

            messageBack = await LogMessageSerializer.ParseMessage(byteStream) as LogMetricMessage<int>;
            Assert.That(messageBack != null, Is.True);
            Debug.Assert(messageBack != null, nameof(messageBack) + " != null");
            
            Assert.That(messageBack.Category, Is.EqualTo(logMessage.Category));
            Assert.That(messageBack.Message, Is.EqualTo(logMessage.Message));
            Assert.That(messageBack.LogLevel, Is.EqualTo(logMessage.LogLevel));
            Assert.That(messageBack.Value, Is.EqualTo(logMessage.Value));
            Assert.That(messageBack.Unit, Is.EqualTo(logMessage.Unit));
        }
        
        [Test]
        public async Task TestConversionOfLogMetricMessageDouble()
        {
            // First message
            var logMessage = new LogMetricMessage<double>
            {
                Category = "This is a test",
                Message = "Yes",
                LogLevel = LogLevel.Debug,
                Value = 4323.8934,
                Unit = "m"
            };

            var bytes = LogMessageSerializer.ConvertMessage(logMessage);
            
            Assert.That(bytes, Is.Not.Null);
            
            var byteStream = new MemoryStream(bytes);

            var messageBack = await LogMessageSerializer.ParseMessage(byteStream) as LogMetricMessage<double>;
            Assert.That(messageBack != null, Is.True);
            Debug.Assert(messageBack != null, nameof(messageBack) + " != null");
            
            Assert.That(messageBack.Category, Is.EqualTo(logMessage.Category));
            Assert.That(messageBack.Message, Is.EqualTo(logMessage.Message));
            Assert.That(messageBack.LogLevel, Is.EqualTo(logMessage.LogLevel));
            Assert.That(messageBack.Value, Is.EqualTo(logMessage.Value));
            Assert.That(messageBack.Unit, Is.EqualTo(logMessage.Unit).Within(1).Ulps);
            
            // Second message
            logMessage = new LogMetricMessage<double>
            {
                Category = "This is a test",
                Message = "Yes",
                LogLevel = LogLevel.Fatal,
                Value = -323.4321,
                Unit = "km"
            };
            
            bytes = LogMessageSerializer.ConvertMessage(logMessage);
            
            Assert.That(bytes, Is.Not.Null);
            
            byteStream = new MemoryStream(bytes);

            messageBack = await LogMessageSerializer.ParseMessage(byteStream) as LogMetricMessage<double>;
            Assert.That(messageBack != null, Is.True);
            Debug.Assert(messageBack != null, nameof(messageBack) + " != null");
            
            Assert.That(messageBack.Category, Is.EqualTo(logMessage.Category));
            Assert.That(messageBack.Message, Is.EqualTo(logMessage.Message));
            Assert.That(messageBack.LogLevel, Is.EqualTo(logMessage.LogLevel));
            Assert.That(messageBack.Value, Is.EqualTo(logMessage.Value));
            Assert.That(messageBack.Unit, Is.EqualTo(logMessage.Unit).Within(1).Ulps);
        }
    }
}