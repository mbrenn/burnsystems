namespace BurnSystems.Logging
{
    public interface ILogMessage
    {
        /// <summary>
        /// Gets or sets the loglevel of the message
        /// </summary>
        LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the category of the message
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Gets or sets the message text to be stored
        /// </summary>
        string Message { get; set; }
    }
}