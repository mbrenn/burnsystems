//-----------------------------------------------------------------------
// <copyright file="ILockable.cs" company="Martin Brenn">
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

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// Dieses Interface wird von allen Objekten implementiert, 
    /// die irgendwie gesperrt werden müssen
    /// </summary>
    public interface ILockable 
    {
        /// <summary>
        /// Sperrt das Objekt für die Synchronisation
        /// </summary>
        void Lock();

        /// <summary>
        /// Entsperrt das Objekt für die Synchronisation
        /// </summary>
        void Unlock();
    }
}
