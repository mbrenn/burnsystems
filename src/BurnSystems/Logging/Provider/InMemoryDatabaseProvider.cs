using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.Logging.Provider
{
    public class InMemoryDatabaseProvider : ILogProvider
    {
        /// <summary>
        /// Stores a singleton that can be used by simple applications
        /// </summary>
        public static InMemoryDatabaseProvider TheOne { get; }= new InMemoryDatabaseProvider();
        
        /// <summary>
        /// Gets the messages that are received
        /// </summary>
        private readonly List<InMemoryLogMessage> _messages = new List<InMemoryLogMessage>();

        /// <summary>
        /// Gets the messages being received
        /// </summary>
        public List<InMemoryLogMessage> Messages
        {
            get
            {
                lock (_messages)
                {
                    return _messages.ToList();
                }
            }
        }

        public void LogMessage(LogMessage logMessage)
        {
            lock (Messages)
            {
                _messages.Add(
                    new InMemoryLogMessage(logMessage, DateTime.Now));
            }
        }
        
        public void ClearLog()
        {
            lock (Messages)
            {
                _messages.Clear();
            }
        }
    }
}