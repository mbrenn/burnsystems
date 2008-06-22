//-----------------------------------------------------------------------
// <copyright file="ThreadWatcherItem.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading;

namespace BurnSystems.Synchronisation
{
    /// <summary>
    /// Diese Delegatstruktur wird für die Benachrichtigung 
    /// von Threadabbrüchen genutzt. 
    /// </summary>
    public delegate void ThreadAbortAction();

    /// <summary>
    /// Diese Hilfsklasse dient als Speicherobjekt für die zu 
    /// überwachenden Threads. 
    /// </summary>
    class ThreadWatcherItem
    {
        /// <summary>
        /// Der zu überwachende Thread
        /// </summary>
        Thread _Thread;

        /// <summary>
        /// Zeitpunkt, an dem der Thread abgebrochen werden soll
        /// </summary>
        DateTime _TimeOut;

        /// <summary>
        /// Dieser Methode wird aufgerufen, wenn der Thread vom
        /// ThreadWatcher hart abgebrochen wird. 
        /// </summary>
        ThreadAbortAction _OnThreadAbort;

        /// <summary>
        /// Der zu überwachende Thread
        /// </summary>
        public Thread Thread
        {
            get { return _Thread; }
            set { _Thread = value; }
        }

        /// <summary>
        /// Zeitpunkt, an dem der Thread abgebrochen werden soll
        /// </summary>
        public DateTime TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }

        /// <summary>
        /// Dieser Methode wird aufgerufen, wenn der Thread vom
        /// ThreadWatcher hart abgebrochen wird. 
        /// </summary>
        public ThreadAbortAction OnThreadAbort
        {
            get { return _OnThreadAbort; }
            set { _OnThreadAbort = value; }
        }

        /// <summary>
        /// Erzeugt ein neues Item
        /// </summary>
        /// <param name="oThread">Thread, der überwacht werden soll.</param>
        /// <param name="dTimeOut">Zeit</param>
        /// <param name="dOnThreadAbort">Delegat, der aufgerufen werden soll,
        /// wenn der Thread abgeborchen werden soll.</param>
        public ThreadWatcherItem(Thread oThread, DateTime dTimeOut,
            ThreadAbortAction dOnThreadAbort)
        {
            _Thread = oThread;
            _TimeOut = dTimeOut;
            _OnThreadAbort = dOnThreadAbort;
        }
    }
}
