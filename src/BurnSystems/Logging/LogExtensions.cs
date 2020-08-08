#nullable enable
namespace BurnSystems.Logging
{
    /// <summary>
    /// Extends the interface ILog with several useful functions
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Logs the level
        /// </summary>
        /// <param name="logger">The logger to be used</param>
        /// <param name="logLevel">The loglevel to be logged</param>
        /// <param name="messageText">Message to be filtered</param>
        /// <param name="category">Category of the</param>
        public static void Log(this ILogger logger, LogLevel logLevel, string messageText, string category)
        {
            var message = new LogMessage
            {
                LogLevel = logLevel,
                Category = category,
                Message = messageText
            };

            logger.Log(message);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string messageText, string category, int value, string unit)
        {
            var message = new LogMetricMessage<int>
            {
                LogLevel = logLevel, 
                Category = category,
                Message = messageText, 
                Value = value,
                Unit = unit
            };

            logger.Log(message);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string messageText, int value, string unit)
        {
            var message = new LogMetricMessage<int>
            {
                LogLevel = logLevel,
                Message = messageText,
                Value = value,
                Unit = unit
            };

            logger.Log(message);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string messageText, string category, double value, string unit)
        {
            var message = new LogMetricMessage<double>
            {
                LogLevel = logLevel,
                Category = category,
                Message = messageText,
                Value = value,
                Unit = unit
            };

            logger.Log(message);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string messageText, double value, string unit)
        {
            var message = new LogMetricMessage<double>
            {
                LogLevel = logLevel,
                Message = messageText,
                Value = value,
                Unit = unit
            };

            logger.Log(message);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string message)
        {
            logger.Log(logLevel, message, string.Empty);
        }

        public static void Trace(this ILogger logger, string message, string category = "")
        {
            logger.Log(LogLevel.Trace, message, category);
        }

        public static void Debug(this ILogger logger, string message, string category = "")
        {
            logger.Log(LogLevel.Debug, message, category);
        }

        public static void Info(this ILogger logger, string message, string category = "")
        {
            logger.Log(LogLevel.Info, message, category);
        }

        public static void Fatal(this ILogger logger, string message, string category = "")
        {
            logger.Log(LogLevel.Fatal, message, category);
        }

        public static void Warn(this ILogger logger, string message, string category = "")
        {
            logger.Log(LogLevel.Warn, message, category);
        }

        public static void Error(this ILogger logger, string message, string category = "")
        {
            logger.Log(LogLevel.Error, message, category);
        }

        public static void Trace(this ILogger logger, string message, int value, string unit, string category = "")
        {
            logger.Log(LogLevel.Trace, message, category, value, unit);
        }

        public static void Debug(this ILogger logger, string message, int value, string unit, string category = "")
        {
            logger.Log(LogLevel.Debug, message, category, value, unit);
        }

        public static void Info(this ILogger logger, string message, int value, string unit, string category = "")
        {
            logger.Log(LogLevel.Info, message, category, value, unit);
        }

        public static void Fatal(this ILogger logger, string message, int value, string unit, string category = "")
        {
            logger.Log(LogLevel.Fatal, message, category, value, unit);
        }

        public static void Warn(this ILogger logger, string message, int value, string unit, string category = "")
        {
            logger.Log(LogLevel.Warn, message, category, value, unit);
        }

        public static void Error(this ILogger logger, string message, int value, string unit, string category = "")
        {
            logger.Log(LogLevel.Error, message, category, value, unit);
        }

        public static void Trace(this ILogger logger, string message, double value, string unit, string category = "")
        {
            logger.Log(LogLevel.Trace, message, category, value, unit);
        }

        public static void Debug(this ILogger logger, string message, double value, string unit, string category = "")
        {
            logger.Log(LogLevel.Debug, message, category, value, unit);
        }

        public static void Info(this ILogger logger, string message, double value, string unit, string category = "")
        {
            logger.Log(LogLevel.Info, message, category, value, unit);
        }

        public static void Fatal(this ILogger logger, string message, double value, string unit, string category = "")
        {
            logger.Log(LogLevel.Fatal, message, category, value, unit);
        }

        public static void Warn(this ILogger logger, string message, double value, string unit, string category = "")
        {
            logger.Log(LogLevel.Warn, message, category, value, unit);
        }

        public static void Error(this ILogger logger, string message, double value, string unit, string category = "")
        {
            logger.Log(LogLevel.Error, message, category, value, unit);
        }
    }
}
