//-----------------------------------------------------------------------
// <copyright file="ThreadWatcher.cs" company="Martin Brenn">
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
using System.Threading;

namespace BurnSystems.Synchronisation
{
    /// <summary>
    /// Diese Klasse verarbeitet Threads und überprüft, ob 
    /// die Threads nach einer gewissen Zeit schon beendet sind. 
    /// Ist dies nicht der Fall, so werden sie hart abgebrochen. 
    /// </summary>
    public static class ThreadWatcher
    {
        /// <summary>
        /// Diese Hilfsklasse ermöglicht das Nutzen der einfachen
        /// using-Syntax zur Überwachung von Threads
        /// </summary>
        class WatchHelper : IDisposable
        {
            /// <summary>
            /// Der Thread, der überwacht wird. 
            /// </summary>
            Thread _Thread;

            /// <summary>
            /// Erzeugt eine neue Instanz
            /// </summary>
            /// <param name="oThread"></param>
            public WatchHelper(Thread oThread)
            {
                _Thread = oThread;
            }

            #region IDisposable Member

            /// <summary>
            /// Diese Methode wird aufgerufen, wenn das 
            /// Objekt weggeworfen wird
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            public void Dispose(bool bDisposing)
            {
                ThreadWatcher.UnwatchThread(_Thread);
            }

            ~WatchHelper()
            {
                Dispose(false);
            }

            #endregion
        }

        /// <summary>
        /// Liste der zu überwachenden Threads. Dieses Objekt
        /// ist auch für die 
        /// </summary>
        static List<ThreadWatcherItem> _WatchedThreads
            = new List<ThreadWatcherItem>();

        /// <summary>
        /// Flag, ob die Threads geprüft werden sollen.
        /// </summary>
        static volatile bool _IsCheckingThreads = false;

        /// <summary>
        /// Diese Loop überwacht die Threads auf Abbruch
        /// </summary>
        static Thread _WatchLoop;

        /// <summary>
        /// Dieses Ereignis wird genutzt, um bei Bedarf 
        /// das Warten auf den Event zu unterbrechen.
        /// </summary>
        static AutoResetEvent _Event =
            new AutoResetEvent(false);

        /// <summary>
        /// Die Zeit, die zwischen zwei Pollingvorgängen 
        /// maximal vergehen kann
        /// </summary>
        static TimeSpan _PollingTime =
            TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// Fügt einen neuen Thread hinzu
        /// </summary>
        /// <param name="oThread">Thread</param>
        /// <param name="oTimeOut">Timeout, ab dem der Thread abgebrochen
        /// werden soll.</param>
        public static IDisposable WatchThread(Thread oThread, TimeSpan oTimeOut)
        {
            return
                WatchThread(oThread, oTimeOut, null);
        }
        /// <summary>
        /// Fügt einen neuen Thread hinzu
        /// </summary>
        /// <param name="oThread">Thread</param>
        /// <param name="oTimeOut">Timeout, ab dem der Thread abgebrochen
        /// werden soll.</param>
        /// <param name="oDelegate">Delegat, der bei einem Abbruch
        /// ausgeführt werden soll.</param>
        public static IDisposable WatchThread(Thread oThread, TimeSpan oTimeOut,
            ThreadAbortAction oDelegate)
        {
            lock (_WatchedThreads)
            {
                _WatchedThreads.Add(
                    new ThreadWatcherItem(
                        oThread,
                        DateTime.Now + oTimeOut,
                        oDelegate));

                if (_WatchedThreads.Count == 1)
                {
                    // Startet den Thread
                    _IsCheckingThreads = true;
                    _WatchLoop = new Thread(WatchLoop);
                    _WatchLoop.IsBackground = true;
                    _WatchLoop.Priority = ThreadPriority.AboveNormal;
                    _WatchLoop.Start();
                }
            }

            return new WatchHelper(oThread);
        }

        /// <summary>
        /// Nimmt einen Thread von der Liste herunter.
        /// </summary>
        /// <param name="oThread">Thread, der von der Beobachtungsliste 
        /// heruntergenommen werden soll.</param>
        private static void UnwatchThread(Thread oThread)
        {
            lock (_WatchedThreads)
            {
                var oItem = _WatchedThreads.Find(
                    x => x.Thread.ManagedThreadId == oThread.ManagedThreadId);
                if (oItem != null)
                {
                    _WatchedThreads.Remove(oItem);
                }

                if (_WatchedThreads.Count == 0)
                {
                    // Kein Thread mehr zu beobachten, stoppe Loop

                    _IsCheckingThreads = false;
                }
            }
            _Event.Set();
        }

        /// <summary>
        /// Diese Threadschleife wird genutzt um die einzelnen Threads 
        /// zu überwachen. 
        /// </summary>
        static void WatchLoop()
        {
            while (_IsCheckingThreads)
            {
                _Event.WaitOne(_PollingTime, false);

                // Überprüft, ob eine der Threads getötet werden soll

                var dNow = DateTime.Now;
                var aoToBeRemoved = new List<ThreadWatcherItem>();

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    // Bei einem aktiven Debugger werden keine Threads getötet
                    continue;
                }

                for (var n = 0; n < _WatchedThreads.Count; n++)
                {
                    var oItem = _WatchedThreads[n];
                    if (oItem.TimeOut < dNow)
                    {
                        // Thread muss getötet werden
                        oItem.Thread.Abort();
                        if (oItem.OnThreadAbort != null)
                        {
                            oItem.OnThreadAbort();
                        }
                    }
                    if (!oItem.Thread.IsAlive)
                    {
                        aoToBeRemoved.Add(oItem);
                    }
                }

                // Entfernt nun die Threads aus der internen Liste
                foreach (var oItem in aoToBeRemoved)
                {
                    _WatchedThreads.Remove(oItem);
                }

                // Wenn kein Thread mehr zu beobachten ist, so 
                // wird dann die Überwachung eingestellt. 
                lock (_WatchedThreads)
                {
                    _IsCheckingThreads =
                        _WatchedThreads.Count != 0;
                    if (!_IsCheckingThreads)
                    {
                        break;
                    }
                }
            }
        }
    }
}
