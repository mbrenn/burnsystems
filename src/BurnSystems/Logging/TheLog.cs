using System;
using BurnSystems.Logging.Provider;

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

        public static void Log(LogLevel logLevel, string message, string category = "")
        {
            Singleton.Log(logLevel,message, category);
        }

        public static void Trace(string message, string category = "")
        {
            Log(LogLevel.Trace, message, category);
        }

        public static void Info(string message, string category = "")
        {
            Log(LogLevel.Info, message, category);
        }

        public static void Fatal(string message, string category = "")
        {
            Log(LogLevel.Fatal, message, category);
        }

        public static void Warn(string message, string category = "")
        {
            Log(LogLevel.Warn, message, category);
        }

        public static void Error(string message, string category = "")
        {
            Log(LogLevel.Error, message, category);
        }

        public static void Debug(string message, string category = "")
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


        /// <summary>
        /// Defines the event that a new message has been logged. This event is thread-safe, only one event is thrown at one time
        /// </summary>
        public static event EventHandler<LogEventArgs> MessageLogged
        {
            add => Singleton.MessageLogged += value;
            remove => Singleton.MessageLogged -= value;
        }
        
        /// <summary>
        /// Sets the log level of the given provider
        /// </summary>
        /// <param name="provider">Provider to be modified</param>
        /// <param name="newLogLevel">New log level of the provider</param>
        public static void SetLogLevel(ILogProvider provider, LogLevel newLogLevel)
        {
            Singleton.SetLogLevel(provider, newLogLevel);
        }
        
        /// <summary>
        /// Sets the log level of the given provider
        /// </summary>
        /// <param name="provider">Provider to be modified</param>
        public static LogLevel GetLogLevel(ILogProvider provider)
        {
            return Singleton.GetLogLevel(provider);
        }
    }
}