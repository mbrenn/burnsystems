//-----------------------------------------------------------------------
// <copyright file="Log.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace BurnSystems.Logging
{
    /// <summary>
    /// The different loglevels from less important to very importang
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Everything will be logged
        /// </summary>
        Everything = 0,
        /// <summary>
        /// Some information, that is only interesting at verbose level
        /// </summary>
        Verbose = 1,
        /// <summary>
        /// Notifications, which are quite interesting
        /// </summary>
        Notify = 2,
        /// <summary>
        /// Messages, which should be evaluated
        /// </summary>
        Message = 3,
        /// <summary>
        /// An action failed and the application can continue normally
        /// </summary>
        Fail = 4,
        /// <summary>
        /// An action failed and the application can continue with some minor
        /// problems or dataloss
        /// </summary>
        Critical = 5,
        /// <summary>
        /// An action failed and the execution of the application was stopped
        /// </summary>
        Fatal = 6
    }
    /// <summary>
    /// The log class can be used to create logs for specific providers.
    /// </summary>
    public class Log : BurnSystems.Logging.ILog
    {
        /// <summary>
        /// Liste von Logprovider
        /// </summary>
        List<ILogProvider> _LogProviders;

        /// <summary>
        /// Loglevel ab dem der Log überhaupt aktiv wird
        /// </summary>
        LogLevel _FilterLevel = LogLevel.Message;

        object _SyncObject = new object();

        /// <summary>
        /// Loglevel ab dem der Log überhaupt aktiv wird
        /// </summary>
        public LogLevel FilterLevel
        {
            get { return _FilterLevel; }
            set { _FilterLevel = value; }
        }

        /// <summary>
        /// Gibt eine Liste von LogProvider zurück
        /// </summary>
        public ILogProvider[] GetLogProviders()
        {
            return _LogProviders.ToArray();            
        }

        /// <summary>
        /// Erstellt ein neues Log
        /// </summary>
        public Log()
        {
            _LogProviders = new List<ILogProvider>();
        }

        /// <summary>
        /// Fügt einen neuen LogProvider hinzu
        /// </summary>
        /// <param name="iLogProvider"></param>
        public void AddLogProvider(ILogProvider iLogProvider)
        {
            iLogProvider.Start();
            _LogProviders.Add(iLogProvider);
        }

        /// <summary>
        /// Removes log provider
        /// </summary>
        /// <param name="iLogProvider"></param>
        public void RemoveLogProvider(ILogProvider iLogProvider)
        {
            int nPosition = _LogProviders.IndexOf(iLogProvider);

            if (nPosition == -1)
            {
                throw new ArgumentException("iLogprovider is not in internal list", "iLogProvider");
            }

            iLogProvider.Shutdown();
            _LogProviders.RemoveAt(nPosition);
        }

        /// <summary>
        /// Removes all log providers and resets all internal variables
        /// </summary>
        public void Reset()
        {
            _FilterLevel = LogLevel.Message;
            new List<ILogProvider>(_LogProviders).ForEach(delegate(ILogProvider iProvider)
                { RemoveLogProvider(iProvider); });
        }

        /// <summary>
        /// Logs entry
        /// </summary>
        /// <param name="oEntry"></param>
        public void LogEntry(LogEntry oEntry)
        {
            lock (_SyncObject)
            {
                if ((int)oEntry.LogLevel >= (int)_FilterLevel)
                {
                    _LogProviders.ForEach(
                        delegate(ILogProvider iProvider) { iProvider.DoLog(oEntry); });
                }
            }
        }

        #region IDisposable Member

        void Dispose(bool bDisposing)
        {
            if (bDisposing)
            {
                Reset();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Log()
        {
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// Singleton
        /// </summary>
        static Log _Singleton;        

        /// <summary>
        /// Gibt ein Singleton für das Logging zurück
        /// </summary>
        public static Log TheLog
        {
            get
            {
                if (_Singleton == null)
                {
                    lock (typeof(Log))
                    {
                        if (_Singleton == null)
                        {
                            _Singleton = new Log();
                        }
                    }
                }
                return _Singleton;
            }
        }
    }
}
