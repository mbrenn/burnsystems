using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.UserExceptionHandler
{
    /// <summary>
    /// Defines the standard exception handler, which will call every UserExceptionHandler
    /// and hopes to return a true result. If none of the UserExceptionHandlers have handled
    /// the exception successfully, the exception will be rethrown
    /// </summary>
    public class StandardExceptionHandling : IExceptionHandling
    {
        /// <summary>
        /// Stores a list of possible handlers
        /// </summary>
        public IEnumerable<IUserExceptionHandler> handlers { get; set; }

        /// <summary>
        /// Initializes a new instance of the StandardExceptionHandling class
        /// </summary>
        /// <param name="handlers">Handlers to be used</param>
        public StandardExceptionHandling(IEnumerable<IUserExceptionHandler> handlers)
        {
            this.handlers = handlers;
        }

        /// <summary>
        /// Handles the exceptions
        /// </summary>
        /// <param name="exc">Exceptions to be handled</param>
        public bool HandleException(Exception exc)
        {
            // Null handler, will not handle exception
            if (handlers == null)
            {
                throw new NotHandledException("Not handled", exc);
            }

            // Try to handle the exception
            var handled = false;
            foreach (var handler in handlers)
            {
                handled |= handler.Handle(exc);
            }

            // Check, whether the exception has not been handled
            if (!handled)
            {
                throw new NotHandledException("Not handled", exc);
            }

            return handled;
        }
    }
}
