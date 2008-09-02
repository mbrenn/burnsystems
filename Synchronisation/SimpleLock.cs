//-----------------------------------------------------------------------
// <copyright file="SimpleLock.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Synchronisation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BurnSystems.Interfaces;

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
        /// Wenn dieses Objekt erzeugt wird, so wird das übergebene
        /// Objekt gesperrt
        /// </summary>
        /// <param name="lockable">Das Objekt, das zu sperren ist</param>
        public SimpleLock(ILockable lockable)
        {
            if (lockable == null)
            {
                throw new ArgumentNullException("lockable");
            }

            this.lockable = lockable;
            this.lockable.Lock();
        }

        /// <summary>
        /// Finaliser of this class
        /// </summary>
        ~SimpleLock()
        {
            this.Dispose(false);
        }

        #region IDisposable Member

        /// <summary>
        /// Entsperrt das Objekt wieder
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes und unlocks the element
        /// </summary>
        /// <param name="disposing">Called by Dispose</param>
        private void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.disposed = true;
                this.lockable.Unlock();
            }
        }       

        #endregion
    }
}
