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

        /// <summary>
        /// Adds a context object, if no object is already existing at the place. 
        /// This is done by check, if an instance can be created
        /// </summary>
        /// <typeparam name="T">Type of the context</typeparam>
        /// <param name="contextSource">Contextsource to whom the object shall be added</param>
        /// <param name="factory">Factory method</param>
        public static void AddIfNotExisting<T>(this IContextSource contextSource, Func<T> factory)
        {
            using (var testContext = new Context(contextSource, "TestContext"))
            {
                var value = testContext.Get<T>();
                if (value == null)
                {
                    contextSource.Add<T>(factory);
                }
            }
        }

        /// <summary>
        /// Adds a context object, if no object is already existing at the place. 
        /// This is done by check, if an instance can be created
        /// </summary>
        /// <typeparam name="T">Type of the context</typeparam>
        /// <param name="contextSource">Contextsource to whom the object shall be added</param>
        /// <param name="token">Token of the object</param>
        /// <param name="factory">Factory method</param>
        public static void AddIfNotExisting<T>(this IContextSource contextSource, string token, Func<T> factory)
        {
            using (var testContext = new Context(contextSource, "TestContext"))
            {
                var value = testContext.Get<T>(token);
                if (value == null)
                {
                    contextSource.Add<T>(token, factory);
                }
            }
        }
    }
}
