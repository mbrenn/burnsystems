using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Scope
{
    /// <summary>
    /// Contains some extension methods for Context
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Tries to add the context source of the given object. 
        /// If the object implements IContextSource or IContextSourceFactory, the object
        /// can be added
        /// </summary>
        /// <param name="context">Context, where Context Source shall be added</param>
        /// <param name="value">Object perhaps containing a context source</param>
        /// <returns>true, if something could be added, otherwise false</returns>
        public static bool TryToAddContextSource(this IContext context, object value)
        {
            var contextSource = value as IContextSource;
            if (contextSource != null)
            {
                context.Add(contextSource);
                return true;
            }

            var contextSourceFactory = value as IContextSourceFactory;
            if (contextSourceFactory != null)
            {
                context.Add(contextSourceFactory.CreateContextSource());
                return true;
            }

            return false;
        }
    }
}
