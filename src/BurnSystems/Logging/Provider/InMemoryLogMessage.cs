using System;
using System.Globalization;

namespace BurnSystems.Logging.Provider
{
    public class InMemoryLogMessage
    {
        /// <summary>
        /// Stores the message being stored in memory
        /// </summary>
        public LogMessage LogMessage;

        /// <summary>
        /// Gets or sets the date at which the log was created
        /// </summary>
        public DateTime Created;

        public InMemoryLogMessage(LogMessage logMessage, DateTime created)
        {
            LogMessage = logMessage;
            Created = created;
        }

        public override string ToString()
        {
            var timePassed = Created - TheLog.TimeCreated;
            if (LogMessage is ILogMetricMessage logMetricMessage)
            {
                return $"{DateTime.Now};{timePassed.TotalSeconds.ToString("n3", CultureInfo.InvariantCulture)};" +
                       $"{LogMessage.LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)};{LogMessage.Category};" +
                       $"{LogMessage.Message};{logMetricMessage.ValueText};{logMetricMessage.Unit}";
            }

            return $"{DateTime.Now};{timePassed.TotalSeconds.ToString("n3", CultureInfo.InvariantCulture)};" +
                   $"{LogMessage.LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)};{LogMessage.Category};" +
                   $"{LogMessage.Message}";

        }
    }
}