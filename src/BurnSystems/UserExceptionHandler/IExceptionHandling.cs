using System;

namespace BurnSystems.UserExceptionHandler
{
    /// <summary>
    /// This application handler has a default implementation and will
    /// call existing IUserExceptionHandlers
    /// </summary>
    public interface IExceptionHandling
    {
        /// <summary>
        /// Handles or throws the exception
        /// </summary>
        /// <param name="exc">Exception to be handled.</param>
        /// <returns>True, if the exception was handled by at least one item</returns>
        bool HandleException(Exception exc);
    }
}
