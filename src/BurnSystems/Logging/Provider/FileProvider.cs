using System;
using System.Globalization;
using System.IO;

namespace BurnSystems.Logging.Provider
{
    public class FileProvider : ILogProvider, IDisposable
    {
        private readonly bool _createNew;
        private readonly string _filePath;
        private readonly object _syncObject = new object();

        private StreamWriter? _file;

        public FileProvider(string filePath, bool createNew = false)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            _filePath = filePath;
            _createNew = createNew;

            var createTry = 0;
            while (createTry < 100 && _file == null)
            {
                try
                {
                    var fileCore = Path.GetFileNameWithoutExtension(_filePath);
                    var fileExtension = Path.GetExtension(_filePath);
                    var directoryPath = Path.GetDirectoryName(_filePath);
                    var number = createTry == 0 ? string.Empty : "." + createTry;

                    var fileName = Path.Combine(directoryPath, fileCore + number + fileExtension);
                    _file = new StreamWriter(fileName, !_createNew);
                }
                catch (Exception)
                {
                    createTry++;
                }
            }

            if (_file == null)
            {
                throw new InvalidOperationException($"File could not be created: {createTry}");
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
                    throw new InvalidOperationException("File-Handle could not be created. ");
                }

                var timePassed = DateTime.Now - TheLog.TimeCreated;

                if (logMessage is ILogMetricMessage logMetricMessage)
                {
                    _file.WriteLine(
                        $"{DateTime.Now};{timePassed.TotalSeconds.ToString("n3", CultureInfo.InvariantCulture)};" +
                        $"{logMessage.LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)};{logMessage.Category};" +
                        $"{logMessage.Message};{logMetricMessage.ValueText};{logMetricMessage.Unit}");
                }
                else
                {
                    _file.WriteLine(
                        $"{DateTime.Now};{timePassed.TotalSeconds.ToString("n3", CultureInfo.InvariantCulture)};" +
                        $"{logMessage.LogLevel.ToString().PaddingRight(Logger.MaxLengthLogLevel)};{logMessage.Category};" +
                        $"{logMessage.Message}");
                }

                _file.Flush();
            }
        }
    }
}