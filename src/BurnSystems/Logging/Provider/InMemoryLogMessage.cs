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

        public override string ToString()
        {
            var timePassed = Created - TheLog.TimeCreated;
            return 
                $"{DateTime.Now};{timePassed.TotalSeconds.ToString("n3", CultureInfo.InvariantCulture)};" +
                $"[{LogMessage.LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)}];{LogMessage.Category};{LogMessage.Message}";

        }
    }
}