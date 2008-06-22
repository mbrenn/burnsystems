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

using System;
using System.Collections.Generic;
using System.Text;

namespace BurnSystems.Synchronisation
{
    /// <summary>
    /// Diese Hilfsklasse vereinfacht den Zugriff auf einen lesenden 
    /// und schreibenden Sperrzugriff, der zur Synchronisation von 
    /// Threads genutzt werden kann. 
    /// </summary>
    public class ReadWriteLock
    {
        System.Threading.ReaderWriterLock _Lock;

        #region Hilfsklassen für die einfache Freigabe des Lese-/Schreibzugriffes

        /// <summary>
        /// Hilfsklasse 
        /// </summary>
        class ReaderLock : IDisposable
        {
            ReadWriteLock _Lock;

            public ReaderLock(ReadWriteLock oLock)
            {
                _Lock = oLock;
            }

            #region IDisposable Member

            /// <summary>
            /// Gibt dieses Objekt wieder frei
            /// </summary>
            public void Dispose(bool bDisposing)
            {
                if (bDisposing)
                {
                    _Lock._Lock.ReleaseReaderLock();
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~ReaderLock()
            {
                Dispose(false);
            }

            #endregion
        }

        /// <summary>
        /// Hilfsklasse für den Schreibzugriff
        /// </summary>
        class WriterLock : IDisposable
        {
            ReadWriteLock _Lock;

            public WriterLock(ReadWriteLock oLock)
            {
                _Lock = oLock;
            }

            #region IDisposable Member
            public void Dispose(bool bDisposing)
            {
                if (bDisposing)
                {
                    _Lock._Lock.ReleaseReaderLock();
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~WriterLock()
            {
                Dispose(false);
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Erstellt ein neues Hilfsobjekt
        /// </summary>
        public ReadWriteLock()
        {
            _Lock = new System.Threading.ReaderWriterLock();
        }

        /// <summary>
        /// Sperrt das Objekt für einen lesegeschützten Zugriff. 
        /// Wenn das zurückgegebene Objekt gelüscht wird, so wird die Sperrung
        /// wieder aufgehoben.
        /// </summary>
        /// <returns>Objekt, dass die Lesefreigabe wieder aufhebt, wenn
        /// dess Methode <c>Dispose</c> aufgerufen wird</returns>
        public IDisposable GetReadLock()
        {
            _Lock.AcquireReaderLock(-1);
            return new ReaderLock(this);
        }

        /// <summary>
        /// Sperrt das Objekt für einen schreibgeschützten Zugriff. 
        /// Wenn das zurückgegebene Objekt gelüscht wird, so wird die Sperrung
        /// wieder aufgehoben.
        /// </summary>
        /// <returns>Objekt, dass die Schreibfreigabe wieder aufhebt, wenn
        /// dess Methode <c>Dispose</c> aufgerufen wird</returns>
        public IDisposable GetWriteLock()
        {
            _Lock.AcquireWriterLock(-1);
            return new WriterLock(this);
        }
    }
}
