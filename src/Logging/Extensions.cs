using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Logging
{
    /// <summary>
    /// Extension methods for logging interface
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Logs an entry
        /// </summary>
        /// <param name="log">Log where entry shall be added</param>
        /// <param name="level">Level to be logged</param>
        /// <param name="entry">Text to be logged</param>
        public static void LogEntry(this ILog log, LogLevel level, string entry)
        {
            log.LogEntry(new LogEntry(entry, level));
        }
    }
}
