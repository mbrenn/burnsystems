using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.Logging
{
    public class Logger : ILogger
    {
        /// <summary>
        /// Gets maximum text length of all log messages
        /// </summary>
        public static int MaxLengthLogLevel { get; }

        /// <summary>
        /// Stores the list of providers
        /// </summary>
        private readonly List<ProviderData> _providers = new List<ProviderData>();

        /// <summary>
        /// Gets or sets the log level threshold for the logging
        /// </summary>
        public LogLevel LogLevelThreshold { get; set; } = LogLevel.Trace;

        /// <summary>
        /// Performs the static initialization
        /// </summary>
        static Logger()
        {
            // Gets the maximum length of the log messages by going through the iteration
            MaxLengthLogLevel = Enum.GetNames(typeof(LogLevel)).Max(x => x.Length);
        }

        /// <summary>
        /// Adds the provider being used for logging
        /// </summary>
        /// <param name="provider">Provider to be added</param>
        /// <param name="logLevelThreshold">Threshold of the log providers.
        /// LogMessage with a lower threshold will not be forwarded to the provider</param>
        public void AddProvider(ILogProvider provider, LogLevel logLevelThreshold)
        {
            var data = new ProviderData(provider,logLevelThreshold);
            _providers.Add(data);
        }

        /// <summary>
        /// Logs the message. All providers within the filter threshold will be notified
        /// </summary>
        /// <param name="message"></param>
        public void Log(LogMessage message)
        {
            var logLevelDepth = (int) message.LogLevel;
            var threshold = (int) LogLevelThreshold;

            if (logLevelDepth < threshold)
            {
                // Do Nothing
                return;
            }

            // Now go through each provider and verify the log messages
            foreach (var provider in _providers)
            {
                var providerLogLevel = (int) provider.LogLevelThreshold;
                if (logLevelDepth < providerLogLevel)
                {
                    continue;
                }

                provider.Provider.LogMessage(message);
            }
        }

        /// <summary>
        /// Stores the provider data being used in the logger
        /// </summary>
        private class ProviderData
        {
            public ProviderData(ILogProvider provider, LogLevel logLevelThreshold)
            {
                Provider = provider;
                LogLevelThreshold = logLevelThreshold;
            }

            public LogLevel LogLevelThreshold { get; set; }

            public ILogProvider Provider { get; set; }
        }

        public void ClearProviders()
        {
            _providers.Clear();
        }
    }
}