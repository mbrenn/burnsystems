using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// Defines the interface, that needs to be implemented
    /// by all exceptiopn handlers, which might log, send or handle an exception of user
    /// </summary>
    public interface IUserExceptionHandler
    {
        /// <summary>
        /// The implementation of the functions handles the exception
        /// </summary>
        /// <param name="exc">Exception to be handled</param>
        /// <returns>true, if the exception has been handled, 
        /// otherwise the exception needs to be rethrown</returns>
        bool Handle(Exception exc);
    }
}
