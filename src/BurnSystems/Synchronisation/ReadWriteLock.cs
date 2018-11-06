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
        private readonly ReaderWriterLockSlim _nativeLockSlim;

        /// <summary>
        /// Native lockstructure for mono
        /// </summary>
        private readonly ReaderWriterLock _nativeLock;

        /// <summary>
        /// Initializes a new instance of the ReadWriteLock class.
        /// If this readwritelock runs within mono, a simple lock will be used
        /// </summary>
        public ReadWriteLock()
        {
            if (EnvironmentHelper.IsMono)
            {
                _nativeLock =
                    new ReaderWriterLock();
            }
            else
            {
                _nativeLockSlim
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
            if (_nativeLockSlim != null)
            {
                _nativeLockSlim.EnterReadLock();
                return new ReaderLockSlim(this);
            }
            else
            {
                _nativeLock.AcquireReaderLock(-1);
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
            if (_nativeLockSlim != null)
            {
                _nativeLockSlim.EnterWriteLock();
                return new WriterLockSlim(this);
            }
            else
            {
                _nativeLock.AcquireWriterLock(-1);
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
            if (disposing && _nativeLockSlim != null)
            {
                _nativeLockSlim.Dispose();
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
            private readonly ReadWriteLock _readWriteLock;

            /// <summary>
            /// Initializes a new instance of the ReaderLockSlim class.
            /// </summary>
            /// <param name="readWriteLock">Read locked structure,
            /// which should be controlled by this lock.</param>
            public ReaderLockSlim(ReadWriteLock readWriteLock)
            {
                this._readWriteLock = readWriteLock;
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
                    _readWriteLock._nativeLockSlim.ExitReadLock();
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
            private readonly ReadWriteLock _readWriteLock;

            /// <summary>
            /// Initializes a new instance of the ReaderLock class.
            /// </summary>
            /// <param name="readWriteLock">Read locked structure,
            /// which should be controlled by this lock.</param>
            public ReaderLock(ReadWriteLock readWriteLock)
            {
                this._readWriteLock = readWriteLock;
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
                    _readWriteLock._nativeLock.ReleaseReaderLock();
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
            private readonly ReadWriteLock _readWriteLock;

            /// <summary>
            /// Initializes a new instance of the WriterLockSlim class.
            /// </summary>
            /// <param name="readWriteLock">Read locked structure,
            /// which should be controlled by this lock.</param>
            public WriterLockSlim(ReadWriteLock readWriteLock)
            {
                this._readWriteLock = readWriteLock;
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
                    _readWriteLock._nativeLockSlim.ExitWriteLock();
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
            private readonly ReadWriteLock _readWriteLock;

            /// <summary>
            /// Initializes a new instance of the WriterLock class.
            /// </summary>
            /// <param name="readWriteLock">Read locked structure,
            /// which should be controlled by this lock.</param>
            public WriterLock(ReadWriteLock readWriteLock)
            {
                this._readWriteLock = readWriteLock;
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
                    _readWriteLock._nativeLock.ReleaseWriterLock();
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
