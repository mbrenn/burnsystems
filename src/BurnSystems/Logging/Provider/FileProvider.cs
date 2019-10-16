using System;
using System.Globalization;
using System.IO;

namespace BurnSystems.Logging.Provider
{
    public class FileProvider : ILogProvider, IDisposable
    {
        private readonly string _filePath;
        private readonly bool _createNew;
        private readonly object _syncObject = new object();

        private StreamWriter? _file;

        public FileProvider(string filePath, bool createNew = false)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            _filePath = filePath;
            _createNew = createNew;
        }

        /// <summary>
        /// Logs the message into the given file. If the file is not open, it will be opened
        /// </summary>
        /// <param name="logMessage"></param>
        public void LogMessage(LogMessage logMessage)
        {
            lock (_syncObject)
            {
                if (_file == null)
                {
                    _file = new StreamWriter(_filePath, !_createNew);
                }

                var timePassed = DateTime.Now - TheLog.TimeCreated;
                _file.WriteLine(
                    $"{DateTime.Now};{timePassed.TotalSeconds.ToString("n3", CultureInfo.InvariantCulture)};" +
                    $"{logMessage.LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)};{logMessage.Category};{logMessage.Message}");
                _file.Flush();
            }
        }

        public void Dispose()
        {
            lock (_syncObject)
            {
                _file?.Dispose();
                _file = null;
            }
        }
    }
}