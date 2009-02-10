//-----------------------------------------------------------------------
// <copyright file="ReadWriteLock.cs" company="Martin Brenn">
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

    /// <summary>
    /// Diese Hilfsklasse vereinfacht den Zugriff auf einen lesenden 
    /// und schreibenden Sperrzugriff, der zur Synchronisation von 
    /// Threads genutzt werden kann. 
    /// </summary>
    public class ReadWriteLock
    {
        /// <summary>
        /// Native lockstructure
        /// </summary>
        private System.Threading.ReaderWriterLockSlim nativeLock
            = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Locks object for readaccess. If returned structure
        /// is disposed, the object will become unlocked
        /// </summary>
        /// <returns>Object controlling the lifetime of readlock</returns>
        public IDisposable GetReadLock()
        {
            this.nativeLock.EnterReadLock();
            return new ReaderLock(this);
        }

        /// <summary>
        /// Locks object for writeaccess. If returned structure
        /// is disposed, the object will become unlocked
        /// </summary>
        /// <returns>Object controlling the lifetime of writelock</returns>
        public IDisposable GetWriteLock()
        {
            this.nativeLock.EnterWriteLock();
            return new WriterLock(this);
        }

        /// <summary>
        /// Helperclass for readerlock
        /// </summary>
        private class ReaderLock : IDisposable
        {
            /// <summary>
            /// Reference to readwritelock-object
            /// </summary>
            private ReadWriteLock readWriteLock;

            /// <summary>
            /// Initializes a new instance of the ReaderLock class.
            /// </summary>
            /// <param name="readWriteLock">Read locked structure,
            /// which should be controlled by this lock.</param>
            public ReaderLock(ReadWriteLock readWriteLock)
            {
                this.readWriteLock = readWriteLock;
            }

            /// <summary>
            /// Finalizes an instance of the ReaderLock class.
            /// </summary>
            ~ReaderLock()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            /// <param name="disposing">Flag, if Dispose() has been called</param>
            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    this.readWriteLock.nativeLock.ExitReadLock();
                }
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Hilfsklasse für den Schreibzugriff
        /// </summary>
        private class WriterLock : IDisposable
        {
            /// <summary>
            /// Reference to readwritelock-object
            /// </summary>
            private ReadWriteLock readWriteLock;

            /// <summary>
            /// Initializes a new instance of the WriterLock class.
            /// </summary>
            /// <param name="readWriteLock">Read locked structure,
            /// which should be controlled by this lock.</param>
            public WriterLock(ReadWriteLock readWriteLock)
            {
                this.readWriteLock = readWriteLock;
            }

            /// <summary>
            /// Finalizes an instance of the WriterLock class.
            /// </summary>
            ~WriterLock()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            /// <param name="disposing">Flag, if Dispose() has been called</param>
            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    this.readWriteLock.nativeLock.ExitWriteLock();
                }
            }
            
            /// <summary>
            /// Dispoeses the object
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
