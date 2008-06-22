//-----------------------------------------------------------------------
// <copyright file="Result.cs" company="Martin Brenn">
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

namespace BurnSystems.Test
{
    /// <summary>
    /// Enthält das Testergebnis eines Unittests
    /// </summary>
    [Serializable()]
    public class Result
    {
        /// <summary>
        /// True, wenn der Test fehlgeschlagen ist.
        /// </summary>
        public bool Failed
        {
            get;
            set;
        }

        /// <summary>
        /// Dauer des Tests
        /// </summary>
        public TimeSpan Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Ausnahme, mit der der Test gescheitert ist. 
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        }
    }
}
