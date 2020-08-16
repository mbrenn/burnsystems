using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Liste der zu überwachenden Threads. Dieses Objekt
        /// ist auch für die 
        /// </summary>
        private static readonly List<ThreadWatcherItem> WatchedThreads
            = new List<ThreadWatcherItem>();

        /// <summary>
        /// Flag, ob die Threads geprüft werden sollen.
        /// </summary>
        private static volatile bool _checkingThreads;

        /// <summary>
        /// Diese Loop überwacht die Threads auf Abbruch
        /// </summary>
        private static Thread? _watchLoop;

        /// <summary>
        /// Dieses Ereignis wird genutzt, um bei Bedarf 
        /// das Warten auf den Event zu unterbrechen.
        /// </summary>
        private static readonly AutoResetEvent ResetEvent =
            new AutoResetEvent(false);

        /// <summary>
        /// Die Zeit, die zwischen zwei Pollingvorgängen 
        /// maximal vergehen kann
        /// </summary>
        private static readonly TimeSpan PollingTime =
            TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// Fügt einen neuen Thread hinzu
        /// </summary>
        /// <param name="thread">Thread to be watched</param>
        /// <param name="timeOut">Timeout, ab dem der Thread abgebrochen
        /// werden soll.</param>
        /// <returns>Disposable interface, which stops the watch
        /// during disposal</returns>
        public static IDisposable WatchThread(Thread thread, TimeSpan timeOut)
        {
            return
                WatchThread(thread, timeOut, null);
        }

        /// <summary>
        /// Fügt einen neuen Thread hinzu
        /// </summary>
        /// <param name="thread">Thread to be watched</param>
        /// <param name="timeOut">Timeout for watching threads
        /// werden soll.</param>
        /// <param name="actionDelegate">Delegate, which is
        /// called if thread is aborted</param>
        /// <returns>Disposable interface, which stops the watch
        /// during disposal</returns>
        public static IDisposable WatchThread(
            Thread thread,
            TimeSpan timeOut,
            ThreadAbortAction? actionDelegate)
        {
            lock (WatchedThreads)
            {
                WatchedThreads.Add(
                    new ThreadWatcherItem(
                        thread,
                        DateTime.Now + timeOut,
                        actionDelegate));

                if (WatchedThreads.Count == 1)
                {
                    // Startet den Thread
                    _checkingThreads = true;
                    _watchLoop = new Thread(WatchLoop)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.AboveNormal,
                        Name = "BurnSystems.ThreadWatcher"
                    };
                    _watchLoop.Start();
                }
            }

            return new WatchHelper(thread);
        }

        /// <summary>
        /// Nimmt einen Thread von der Liste herunter.
        /// </summary>
        /// <param name="thread">Thread, der von der Beobachtungsliste 
        /// heruntergenommen werden soll.</param>
        private static void UnwatchThread(Thread thread)
        {
            lock (WatchedThreads)
            {
                var item = WatchedThreads.Find(
                    x => x.Thread.ManagedThreadId == thread.ManagedThreadId);
                if (item != null)
                {
                    WatchedThreads.Remove(item);
                }

                if (WatchedThreads.Count == 0)
                {
                    // Kein Thread mehr zu beobachten, stoppe Loop
                    _checkingThreads = false;
                }
            }

            ResetEvent.Set();
        }

        /// <summary>
        /// Diese Threadschleife wird genutzt um die einzelnen Threads 
        /// zu überwachen. 
        /// </summary>
        private static void WatchLoop()
        {
            while (_checkingThreads)
            {
                ResetEvent.WaitOne(PollingTime, false);

                lock (WatchedThreads)
                {
                    // Überprüft, ob eine der Threads getötet werden soll
                    var now = DateTime.Now;
                    var threadsToBeRemoved = new List<ThreadWatcherItem>();

                    if (Debugger.IsAttached)
                    {
                        // Bei einem aktiven Debugger werden keine Threads getötet
                        continue;
                    }

                    for (var n = 0; n < WatchedThreads.Count; n++)
                    {
                        var item = WatchedThreads[n];
                        if (item == null)
                        {
                            continue;
                        }

                        if (item.TimeOut < now)
                        {
                            // Thread muss getötet werden

                            item.Thread.Abort();
                            item.OnThreadAbort?.Invoke();
                        }

                        if (!item.Thread.IsAlive)
                        {
                            threadsToBeRemoved.Add(item);
                        }
                    }

                    // Entfernt nun die Threads aus der internen Liste
                    foreach (var item in threadsToBeRemoved)
                    {
                        WatchedThreads.Remove(item);
                    }

                    // Wenn kein Thread mehr zu beobachten ist, so 
                    // wird dann die Überwachung eingestellt. 
                    _checkingThreads =
                        WatchedThreads.Count != 0;
                    if (!_checkingThreads)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Diese Hilfsklasse ermöglicht das Nutzen der einfachen
        /// using-Syntax zur Überwachung von Threads
        /// </summary>
        private class WatchHelper : IDisposable
        {
            /// <summary>
            /// Der Thread, der überwacht wird. 
            /// </summary>
            private readonly Thread _thread;

            /// <summary>
            /// Initializes a new instance of the WatchHelper class.
            /// </summary>
            /// <param name="thread">Thread to be watched</param>
            public WatchHelper(Thread thread)
            {
                _thread = thread;
            }

            /// <summary>
            /// Finalizes an instance of the WatchHelper class.
            /// </summary>
            ~WatchHelper()
            {
                Dispose();
            }

            #region IDisposable Member

            /// <summary>
            /// Diese Methode wird aufgerufen, wenn das 
            /// Objekt weggeworfen wird
            /// </summary>
            public void Dispose()
            {
                UnwatchThread(_thread);
                GC.SuppressFinalize(this);
            }

            #endregion
        }
    }
}
