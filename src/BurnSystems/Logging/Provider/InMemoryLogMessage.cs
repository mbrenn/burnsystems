using System;

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
    }
}