namespace BurnSystems.Logging
{
    /// <summary>
    /// Defines the content of the log message
    /// </summary>
    public class LogMessage : ILogMessage
    {
        /// <summary>
        /// Gets or sets the loglevel of the message
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the category of the message
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message text to be stored
        /// </summary>
        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Category))
            {
                return $"[{LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)}]: {Message}";
            }

            return $"[{LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)}] {Category}: {Message}";
        }
    }
}