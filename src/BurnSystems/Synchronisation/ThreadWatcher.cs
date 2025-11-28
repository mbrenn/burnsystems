namespace BurnSystems.Synchronisation
{
    /// <summary>
    /// Diese Klasse verarbeitet Threads und überprüft, ob 
    /// die Threads nach einer gewissen Zeit schon beendet sind. 
    /// Ist dies nicht der Fall, so werden sie hart abgebrochen. 
    /// </summary>
    [Obsolete("Thread Watcher is not supported anymore. Thread.Abort is removed in .Net Core")]
    public static class ThreadWatcher
    {
        /// <summary>
        /// Liste der zu überwachenden Threads. Dieses Objekt
        /// ist auch für die 
        /// </summary>
        private static readonly List<ThreadWatcherItem> WatchedThreads = new();

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
            throw new PlatformNotSupportedException(
                "Thread Watcher is not supported anymore. Thread.Abort is removed in .Net Core");
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
            
            throw new PlatformNotSupportedException(
                "Thread Watcher is not supported anymore. Thread.Abort is removed in .Net Core");
        }

        /// <summary>
        /// Nimmt einen Thread von der Liste herunter.
        /// </summary>
        /// <param name="thread">Thread, der von der Beobachtungsliste 
        /// heruntergenommen werden soll.</param>
        private static void UnwatchThread(Thread thread)
        {
            throw new PlatformNotSupportedException(
                "Thread Watcher is not supported anymore. Thread.Abort is removed in .Net Core");
        }

        /// <summary>
        /// Diese Threadschleife wird genutzt um die einzelnen Threads 
        /// zu überwachen. 
        /// </summary>
        private static void WatchLoop()
        {
            throw new PlatformNotSupportedException(
                "Thread Watcher is not supported anymore. Thread.Abort is removed in .Net Core");
        }
    }
}
