using System;
using System.Runtime.Serialization;

namespace BurnSystems.Test
{
    /// <summary>
    /// This method is thrown, when an ensure check failes
    /// </summary>
    [Serializable]
    public class EnsureFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the EnsureFailedException class.
        /// </summary>
        public EnsureFailedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the EnsureFailedException class.
        /// </summary>
        /// <param name="message">Message, why check failed</param>
        public EnsureFailedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EnsureFailedException class.
        /// </summary>
        /// <param name="message">Message of check</param>
        /// <param name="inner">Inner exception</param>
        public EnsureFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EnsureFailedException class.
        /// </summary>
        /// <param name="info">Used by serialization</param>
        /// <param name="context">Context required for serialization</param>
        protected EnsureFailedException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }
    }
}

