//-----------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This is a log entry
    /// </summary>
    [Serializable()]
    public class LogEntry
    {
        /// <summary>
        /// Initializes a new instance of the LogEntry class.
        /// </summary>
        /// <param name="message">Message to be stored</param>
        /// <param name="logLevel">Loglevel of this entry</param>
        public LogEntry(string message, LogLevel logLevel)
        {
            this.Message = message;
            this.LogLevel = logLevel;
            this.Categories = string.Empty;
            this.Created = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the LogEntry class.
        /// </summary>
        /// <param name="message">Message to be stored</param>
        /// <param name="logLevel">Loglevel of this entry</param>
        /// <param name="categories">Commaseparated list of categories</param>
        public LogEntry(string message, LogLevel logLevel, string categories)
        {
            this.Message = message;
            this.LogLevel = logLevel;
            this.Categories = categories;
            this.Created = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the datetime, when entry was created
        /// </summary>
        public DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the logleved
        /// </summary>
        public LogLevel LogLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a comma separated list of categories
        /// </summary>
        public string Categories
        {
            get;
            set;
        }
    }
}
