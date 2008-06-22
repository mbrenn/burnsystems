//-----------------------------------------------------------------------
// <copyright file="EnsureFailedException.cs" company="Martin Brenn">
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
    [global::System.Serializable]
    public class EnsureFailedException : Exception
    {

        public EnsureFailedException() { }
        public EnsureFailedException(string message) : base(message) { }
        public EnsureFailedException(string message, Exception inner) : base(message, inner) { }
        protected EnsureFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
