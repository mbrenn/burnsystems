namespace BurnSystems.Synchronisation
{
    using System;
    using Interfaces;
    using Test;

    /// <summary>
    /// Eine einfache Locking-Klasse, die für das Locken 
    /// eines ILockable-Objektes zustündig ist
    /// </summary>
    public class SimpleLock : IDisposable
    {
        /// <summary>
        /// The lockable structure, which is used by this simple lock
        /// </summary>
        private ILockable lockable;

        /// <summary>
        /// Flag, if object was disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the SimpleLock class. The object
        /// <c>lockable</c> is locked. 
        /// </summary>
        /// <param name="lockable">Das Objekt, das zu sperren ist</param>
        public SimpleLock(ILockable lockable)
        {
            Ensure.IsNotNull(lockable);

            this.lockable = lockable;
            this.lockable.Lock();
        }

        /// <summary>
        /// Finalizes an instance of the SimpleLock class.
        /// </summary>
        ~SimpleLock()
        {
            Dispose(false);
        }

        #region IDisposable Member

        /// <summary>
        /// Entsperrt das Objekt wieder
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes und unlocks the element
        /// </summary>
        /// <param name="disposing">Called by Dispose</param>
        private void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;
                lockable.Unlock();
            }
        }       

        #endregion
    }
}
