namespace BurnSystems.Logging
{
    /// <summary>
    /// Defines the interface that each provider needs to implement.
    /// A provider receives each log message and is allowed to handle the log message
    /// as it would like
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Logs the message
        /// </summary>
        /// <param name="logMessage">Message to be logged</param>
        void LogMessage(LogMessage logMessage);
    }
}