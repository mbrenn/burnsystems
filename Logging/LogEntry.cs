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

using System;
using System.Collections.Generic;
using System.Text;

namespace BurnSystems.Logging
{
    /// <summary>
    /// This is a log entry
    /// </summary>
    [Serializable()]
    public class LogEntry
    {
        String _Message;
        DateTime _Created;
        LogLevel _LogLevel;
        String _Categories;

        /// <summary>
        /// Message
        /// </summary>
        public String Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        /// <summary>
        /// Time, when this log entry was created
        /// </summary>
        public DateTime Created
        {
            get { return _Created; }
            set { _Created = value; }
        }

        /// <summary>
        /// Loglevel
        /// </summary>
        public LogLevel LogLevel
        {
            get { return _LogLevel; }
            set { _LogLevel = value; }
        }

        /// <summary>
        /// A komma separated list of categories
        /// </summary>
        public String Catagories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }


        /// <summary>
        /// Creates new logentry
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="logLevel">Loglevel of this entry</param>
        public LogEntry(String message, LogLevel logLevel)
        {
            Message = message;
            LogLevel = logLevel;
            Catagories = "";
            Created = DateTime.Now;
        }

        /// <summary>
        /// Creates new logentry
        /// </summary>
        /// <param name="message">Message to be stored</param>
        /// <param name="logLevel">Loglevel of this entry</param>
        /// <param name="categories">Categories</param>
        public LogEntry(String message, LogLevel logLevel, String categories)
        {
            Message = message;
            LogLevel = logLevel;
            Catagories = categories;
            Created = DateTime.Now;
        }
    }
}
