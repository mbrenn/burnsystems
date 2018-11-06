using System;
using System.Data.Common;

namespace BurnSystems.Logging
{
    public static class TheLog
    {
        /// <summary>
        /// Gets the singleton
        /// </summary>
        internal static Logger Singleton { get; } = new Logger();

        /// <summary>
        /// Gets or sets the time when the first logger was created
        /// </summary>
        internal static DateTime TimeCreated { get; }

        /// <summary>
        /// Initializes the log to store the time when the process was created
        /// </summary>
        static TheLog()
        {
            TimeCreated = DateTime.Now;
        }

        /// <summary>
        /// Logs the message into the singleton
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Log(LogMessage message)
        {
            Singleton.Log(message);
        }

        public static void Log(LogLevel logLevel, string message, string category)
        {
            Singleton.Log(logLevel,message, category);
        }

        public static void Log(LogLevel logLevel, string message)
        {
            Singleton.Log(logLevel, message, string.Empty);
        }

        public static void Trace(string message, string category = null)
        {
            Log(LogLevel.Trace, message, category);
        }

        public static void Info(string message, string category = null)
        {
            Log(LogLevel.Info, message, category);
        }

        public static void Fatal(string message, string category = null)
        {
            Log(LogLevel.Fatal, message, category);
        }

        public static void Warn(string message, string category = null)
        {
            Log(LogLevel.Warn, message, category);
        }

        public static void Error(string message, string category = null)
        {
            Log(LogLevel.Error, message, category);
        }

        public static void Debug(string message, string category = null)
        {
            Log(LogLevel.Debug, message, category);
        }

        public static void AddProvider(ILogProvider logProvider, LogLevel logLevelThreshold = LogLevel.Info)
        {
            Singleton.AddProvider(logProvider, logLevelThreshold);
        }

        public static void ClearProviders()
        {
            Singleton.ClearProviders();
        }

        /// <summary>
        /// Gets or sets the filter threshold for the logging
        /// </summary>
        public static LogLevel FilterThreshold
        {
            get => Singleton.LogLevelThreshold;
            set => Singleton.LogLevelThreshold = value;
        }
    }
}