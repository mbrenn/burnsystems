using System;

namespace BurnSystems.Logging.Provider
{
    /// <summary>
    /// Receives the log message and sends out an event to the receiving instance
    /// </summary>
    public class EventProvider : ILogProvider
    {
        /// <summary>
        /// This event is thrown, when a log message is received
        /// </summary>
        public event EventHandler<LogEventArgs> MessageReceived;

        protected virtual void OnMessageReceived(LogEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Pushes the log message to the event
        /// </summary>
        /// <param name="logMessage">Message to be sent out</param>
        public void LogMessage(LogMessage logMessage)
        {
            OnMessageReceived(new LogEventArgs(logMessage));
        }
    }

    /// <summary>
    /// Defines the event arguments
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the message being sent out
        /// </summary>
        public LogMessage LogMessage { get; }

        public LogEventArgs(LogMessage logMessage)
        {
            LogMessage = logMessage;
        }
    }
}