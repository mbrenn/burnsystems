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

namespace BurnSystems.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This method is thrown, when an ensure check failes
    /// </summary>
    [global::System.Serializable]
    public class EnsureFailedException : Exception
    {
        /// <summary>
        /// Creates new instance
        /// </summary>
        public EnsureFailedException()
        { 
        }

        /// <summary>
        /// Creates new instance and sets the message
        /// </summary>
        /// <param name="message">Message, why check failed</param>
        public EnsureFailedException(string message) : base(message) 
        { 
        }

        /// <summary>
        /// Creates new instance and sets message and inner exception
        /// </summary>
        /// <param name="message">Message of check</param>
        /// <param name="inner">Inner exception</param>
        public EnsureFailedException(string message, Exception inner) 
            : base(message, inner) 
        { 
        }

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="info">Used by serialization</param>
        /// <param name="context">Context required for serialization</param>
        protected EnsureFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) 
        { 
        }
    }
}
