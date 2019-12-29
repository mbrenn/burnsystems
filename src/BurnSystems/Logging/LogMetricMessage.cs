using System;
using System.Globalization;

namespace BurnSystems.Logging
{
    /// <summary>
    /// Defines the value of a metric message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogMetricMessage<T> : LogMessage, ILogMetricMessage where T : struct
    {
        /// <summary>
        /// Gets or sets the unit of the message according SI units
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value that is sent out
        /// </summary>
        public T Value { get; set; }

        static LogMetricMessage()
        {
            var type = typeof(T);
            if (type != typeof(int)
                && type != typeof(double))
            {
                throw new InvalidOperationException("Only types for int and double are supported.");
            }
        }
        
        /// <summary>
        /// Gets the text of the Value as a text to be used for human parseable logs.
        /// </summary>
        public string ValueText => 
            typeof(T) == typeof(int) ? ((int) (object) Value).ToString(CultureInfo.InvariantCulture) :
            typeof(T) == typeof(double) ? ((double) (object) Value).ToString(CultureInfo.InvariantCulture) :
            Value.ToString();

        public override string ToString()
        {
            Message ??= string.Empty;

            if (string.IsNullOrEmpty(Category))
            {
                return
                    $"[{LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)}]: {Message} ## {ValueText} # {Unit}";
            }

            return
                $"[{LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)}] {Category}: {Message} ## {ValueText} # {Unit}";
        }
    }
}