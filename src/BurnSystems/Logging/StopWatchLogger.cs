using System;
using System.Diagnostics;
using System.Threading;

namespace BurnSystems.Logging
{
    /// <summary>
    /// Implements a stopwatch logger which can be easily used by the logging to have automatic indication of duration of happenings
    /// </summary>
    public class StopWatchLogger : IDisposable
    {
        private readonly ILogger _logger;
        private readonly string _message;
        private readonly LogLevel _logLevel;
        private readonly Stopwatch _stopWatch;
        private bool _isStopped;

        /// <summary>
        /// Defines the already created number of items
        /// </summary>
        private static int _instanceCount = 0;

        /// <summary>
        /// Defines the number of the instance
        /// </summary>
        private readonly int _instance;

        /// <summary>
        /// Initializes a new instance of the stopwatch and sends out first message
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        public StopWatchLogger(ILogger logger, string message, LogLevel logLevel = LogLevel.Info)
        {
            _logger = logger;
            _message = message;
            _logLevel = logLevel;
            _stopWatch = Stopwatch.StartNew();
            _instance = Interlocked.Increment(ref _instanceCount);

            logger.Log(logLevel, $"#{_instance}: Start: {message}");
        }

        public void IntermediateLog(string type)
        {
            _logger.Log(_logLevel, $"#{_instance}: {type}  : {_message}", _stopWatch.ElapsedMilliseconds, "ms");

        }

        /// <summary>
        /// Stops the timing and reports the message
        /// </summary>
        public void Stop()
        {
            _logger.Log(_logLevel, $"#{_instance}: End  : {_message}", _stopWatch.ElapsedMilliseconds, "ms");
            _isStopped = true;
        }

        public void Dispose()
        {
            if (!_isStopped)
            {
                Stop();
            }
        }
    }
}