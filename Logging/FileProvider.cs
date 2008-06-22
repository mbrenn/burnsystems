//-----------------------------------------------------------------------
// <copyright file="FileProvider.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading;

namespace BurnSystems.Logging
{
    /// <summary>
    /// This is a file provider, which stores the logs into a file.
    /// The logging file is only opened during the writing of a log entry
    /// One entry gets one line. 
    /// 01.01.2000 15:43;Verbose;Category1,Category2;Message
    /// </summary>
    public class FileProvider : ILogProvider
    {
        readonly String _Path;

        /// <summary>
        /// Flag, if the Fileprovider is currently in the exception
        /// </summary>
        static bool _InException;

        /// <summary>
        /// Gibt den Pfad zurück an dem das Logging gespeichert wird.
        /// </summary>
        public String Path
        {
            get { return _Path; }
        }

        /// <summary>
        /// Creates new fileprovider with the name
        /// </summary>
        /// <param name="filename">Filename</param>
        public FileProvider(String strPath)
        {
            _Path = strPath;
        }

        #region ILogProvider Members

        /// <summary>
        /// Starts this handler, is doing nothing
        /// </summary>
        public void Start()
        {
            
        }

        /// <summary>
        /// Stops this handler, is doing nothing
        /// </summary>
        public void Shutdown()
        {
            
        }

        /// <summary>
        /// Writes the logentry to the file
        /// </summary>
        /// <param name="entry">Entry to be stored</param>
        public void DoLog(LogEntry entry)
        {
            try
            {
                using (StreamWriter oWriter = new StreamWriter(_Path, true))
                {
                    String strMessage = entry.Message.Replace(';', ',')
                        .Replace('\r', ' ').Replace('\n', ' ');
                    oWriter.WriteLine("{0};{1};{2};{3}",
                        entry.Created, entry.LogLevel, entry.Catagories, strMessage);
                }
            }
            catch (IOException)
            {
                // Perhaps a blocked file
                Thread.Sleep(1000);

                if (!_InException)
                {
                    _InException = true;
                    Log.TheLog.LogEntry(
                        new LogEntry(
                            LocalizationBS.FileLogProvider_ExceptionCaught, LogLevel.Fail));
                }
                _InException = false;

                // Retry
                using (StreamWriter oWriter = new StreamWriter(_Path, true))
                {
                    String strMessage = entry.Message.Replace(';', ',')
                        .Replace('\r', ' ').Replace('\n', ' ');
                    oWriter.WriteLine("{0};{1};{2};{3}",
                        entry.Created, entry.LogLevel, entry.Catagories, strMessage);
                }
            }
        }

        #endregion
    }
}
