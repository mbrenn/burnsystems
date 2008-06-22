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

using System;
using System.Collections.Generic;
using System.Text;
using BurnSystems.Interfaces;

namespace BurnSystems.Synchronisation
{
    /// <summary>
    /// Eine einfache Locking-Klasse, die für das Locken 
    /// eines ILockable-Objektes zustündig ist
    /// </summary>
    public class SimpleLock : IDisposable
    {
        ILockable _Lockable;
        bool _IsDisposed;

        /// <summary>
        /// Wenn dieses Objekt erzeugt wird, so wird das übergebene
        /// Objekt gesperrt
        /// </summary>
        /// <param name="iLockable">Das Objekt, das zu sperren ist</param>
        public SimpleLock(ILockable iLockable)
        {
            if (iLockable == null)
            {
                throw new ArgumentNullException("iLockable");
            }
            _Lockable = iLockable;
            _Lockable.Lock();
        }


        #region IDisposable Member

        void Dispose(bool bDisposing)
        {
            if (bDisposing && !_IsDisposed)
            {
                _IsDisposed = true;
                _Lockable.Unlock();
            }
        }
        /// <summary>
        /// Entsperrt das Objekt wieder
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SimpleLock()
        {
            Dispose(false);
        }

        #endregion
    }
}
