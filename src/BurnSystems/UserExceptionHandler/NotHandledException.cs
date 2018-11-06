using System;

namespace BurnSystems.UserExceptionHandler
{
    /// <summary>
    /// This exception is thrown, when the exception handling does not handle the exception
    /// </summary>
    [Serializable]
    public class NotHandledException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NotHandledException class
        /// </summary>
        public NotHandledException() { }

        /// <summary>
        /// Initializes a new instance of the NotHandledException class
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public NotHandledException(string message) : base(message) { }
        
        /// <summary>
        /// Initializes a new instance of the NotHandledException class
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="inner">Inner exception</param>
        public NotHandledException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the NotHandledException class
        /// </summary>
        /// <param name="info">Information about serialization</param>
        /// <param name="context">Information about streaming context</param>
        protected NotHandledException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
