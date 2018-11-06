namespace BurnSystems.Logging
{
    /// <summary>
    /// Defines the content of the log message
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// Gets or sets the loglevel of the message
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the category of the message
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the message text to be stored
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Category))
            {
                return $"[{LogLevel}]: {Message}";
            }

            return $"[{LogLevel}] {Category}: {Message}";
        }
    }
}