using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace BurnSystems.Logging.Provider
{
    [EventSource(Name="BurnSystems")]
    public class EventTracingProvider : EventSource, ILogProvider
    {
        /// <summary>
        /// Stores the categories of the event
        /// </summary>
        private readonly Dictionary<string, int> _categories = new Dictionary<string, int>();

        [NonEvent]
        public void LogMessage(LogMessage logMessage)
        {
            int eventId;
            lock(_categories)
            {
                if (string.IsNullOrEmpty(logMessage.Category) || logMessage.Category == null)
                {
                    eventId = 1;
                }
                else
                {
                    if (!_categories.TryGetValue(logMessage.Category, out eventId))
                    {
                        eventId = Math.Min(65535, _categories.Count + 2);
                        _categories[logMessage.Category] = eventId;
                    }
                }
            }

            InternalLog(eventId, logMessage.Category ?? string.Empty, logMessage.ToString());
        }

        [Event(1, Message = "{0}, {1}:{2}", Level = EventLevel.Informational)]
        public void InternalLog(int eventId, string category, string message)
        {
            if (IsEnabled())
            {
                WriteEvent(1, eventId, category, message);
            }
        }    
    }
}