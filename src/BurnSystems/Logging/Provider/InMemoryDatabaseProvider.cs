using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Synchronisation;

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
        
        private readonly ReadWriteLock _lock = new ReadWriteLock();

        /// <summary>
        /// Gets the messages being received
        /// </summary>
        public IList<InMemoryLogMessage> Messages
        {
            get
            {
                using (_lock.GetReadLock())
                {
                    return _messages.ToList();
                }
            }
        }

        public void LogMessage(LogMessage logMessage)
        {
            using (_lock.GetWriteLock())
            {
                _messages.Add(
                    new InMemoryLogMessage(logMessage, DateTime.Now));
            }
        }
        
        public void ClearLog()
        {
            using (_lock.GetWriteLock())
            {
                _messages.Clear();
            }
        }
    }
}