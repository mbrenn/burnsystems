namespace BurnSystems.Synchronisation
{
    using System;
    using System.Threading;

    /// <summary>
    /// This helperclass supports the use of readwrite locks
    /// as a disposable pattern. 
    /// In .Net ReaderWriterLockSlim is used, in Mono ReaderWriterLock, because
    /// Mono does not support recursions in the slim lock.
    /// </summary>
    public class ReadWriteLock : IDisposable
    {
        /// <summary>
        /// Native lockstructure
        /// </summary>
        private readonly ReaderWriterLockSlim nativeLockSlim;

        /// <summary>
        /// Native lockstructure for mono
        /// </summary>
        private readonly ReaderWriterLock nativeLock;

        /// <summary>
        /// Initializes a new instance of the ReadWriteLock class.
        /// If this readwritelock runs within mono, a simple lock will be used
        /// </summary>
        public ReadWriteLock()
        {
            if (EnvironmentHelper.IsMono)
            {
                nativeLock =
                    new ReaderWriterLock();
            }
            else
            {
                nativeLockSlim
                    = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            }
        }

        /// <summary>
        /// Locks object for readaccess. If returned structure
        /// is disposed, the object will become unlocked
        /// </summary>
        /// <returns>Object controlling the lifetime of readlock</returns>
        public IDisposable GetReadLock()
        {
            if (nativeLockSlim != null)
            {
                nativeLockSlim.EnterReadLock();
                return new ReaderLockSlim(this);
            }
            else
            {
                nativeLock.AcquireReaderLock(-1);
                return new ReaderLock(this);
            }
        }

        /// <summary>
        /// Locks object for writeaccess. If returned structure
        /// is disposed, the object will become unlocked
        /// </summary>
        /// <returns>Object controlling the lifetime of writelock</returns>
        public IDisposable GetWriteLock()
        {
            if (nativeLockSlim != null)
            {
                nativeLockSlim.EnterWriteLock();
                return new WriterLockSlim(this);
            }
            else
            {
                nativeLock.AcquireWriterLock(-1);
                return new WriterLock(this);
            }
        }

        /// <summary>
        /// Disposes the nativelockslim
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the instance
        /// </summary>
        /// <param name="disposing">True, if called by dispose</param>
        private void Dispose(bool disposing)
        {
            if (disposing && nativeLockSlim != null)
            {
                nativeLockSlim.Dispose();
            }
        }

        /// <summary>
        /// Helper class for disposing the readlock
        /// </summary>
        private class ReaderLockSlim : IDisposable
        {
            /// <summary>
            /// Reference to readwritelock-object
            /// </summary>
            private ReadWriteLock readWriteLock;

            /// <summary>
            /// Initializes a new instance of the ReaderLockSlim class.
            /// </summary>
            /// <param name="readWriteLock">Read locked structure,
            /// which should be controlled by this lock.</param>
            public ReaderLockSlim(ReadWriteLock readWriteLock)
            {
                this.readWriteLock = readWriteLock;
            }

            /// <summary>
            /// Finalizes an instance of the ReaderLockSlim class.
            /// </summary>
            ~ReaderLockSlim()
            {
                Dispose(false);
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            /// <param name="disposing">Flag, if Dispose() has been called</param>
            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    readWriteLock.nativeLockSlim.ExitReadLock();
                }
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Helper class for disposing the readlock
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
                Dispose(false);
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            /// <param name="disposing">Flag, if Dispose() has been called</param>
            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    readWriteLock.nativeLock.ReleaseReaderLock();
                }
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Helper class for disposing the writelock
        /// </summary>
        private class WriterLockSlim : IDisposable
        {
            /// <summary>
            /// Reference to readwritelock-object
            /// </summary>
            private ReadWriteLock readWriteLock;

            /// <summary>
            /// Initializes a new instance of the WriterLockSlim class.
            /// </summary>
            /// <param name="readWriteLock">Read locked structure,
            /// which should be controlled by this lock.</param>
            public WriterLockSlim(ReadWriteLock readWriteLock)
            {
                this.readWriteLock = readWriteLock;
            }

            /// <summary>
            /// Finalizes an instance of the WriterLockSlim class.
            /// </summary>
            ~WriterLockSlim()
            {
                Dispose(false);
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            /// <param name="disposing">Flag, if Dispose() has been called</param>
            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    readWriteLock.nativeLockSlim.ExitWriteLock();
                }
            }
            
            /// <summary>
            /// Dispoeses the object
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Helper class for disposing the writelock
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
                Dispose(false);
            }

            /// <summary>
            /// Disposes the object
            /// </summary>
            /// <param name="disposing">Flag, if Dispose() has been called</param>
            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    readWriteLock.nativeLock.ReleaseWriterLock();
                }
            }

            /// <summary>
            /// Dispoeses the object
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
