﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void HandleException(Exception exc);
    }
}
