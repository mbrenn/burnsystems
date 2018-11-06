using System;
using System.Collections.Generic;

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
        public List<InMemoryLogMessage> Messages { get; }= new List<InMemoryLogMessage>();

        public void LogMessage(LogMessage logMessage)
        {
            lock (Messages)
            {
                Messages.Add(
                    new InMemoryLogMessage()
                    {
                        LogMessage = logMessage,
                        Created = DateTime.Now
                    });
            }
        }
        
        public void ClearLog()
        {
            lock (Messages)
            {
                Messages.Clear();
            }
        }
    }
}