namespace BurnSystems.Logging
{
    public interface ILogMetricMessage
    {
        /// <summary>
        /// Gets or sets the unit of the message according SI units
        /// </summary>
        string Unit { get; set; }

        /// <summary>
        /// Gets the text of the Value as a text to be used for human parseable logs.
        /// </summary>
        string ValueText { get; }
    }
}