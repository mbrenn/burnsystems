﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Test;

namespace BurnSystems.Scope
{
    /// <summary>
    /// Combines a context with an existing context source
    /// </summary>
    public class CombinedContext : IContext
    {
        /// <summary>
        /// Primary context
        /// </summary>
        private IContext primaryContext;

        /// <summary>
        /// Secondary source
        /// </summary>
        private IContext secondaryContext;

        public CombinedContext(IContext context, IContext source)
        {
            Ensure.IsNotNull(context);
            Ensure.IsNotNull(source);
            this.primaryContext = context;
            this.secondaryContext = source;
        }

        /// <summary>
        /// Adds an item to context
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="item">Item to be added</param>
        public void Add<T>(T item)
        {
            this.primaryContext.Add<T>(item);
        }

        /// <summary>
        /// Adds an item to context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="item"></param>
        public void Add<T>(string token, T item)
        {
            this.primaryContext.Add<T>(token, item);
        }

        /// <summary>
        /// Gets an object, it is created if necessary.
        /// This method only works, if there is just one 
        /// instance within the context source
        /// </summary>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <returns>Object to be created or retrieved</returns>
        public T Get<T>()
        {
            var found = this.primaryContext.Get<T>();

            if (found == null)
            {
                found = this.secondaryContext.Get<T>();
            }

            return found;
        }

        /// <summary> 
        /// Gets an object by token. This object is created if necessary.
        /// </summary>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <param name="token">Token to be added</param>
        /// <returns>Object to be created or retrieved</returns>
        public T Get<T>(string token)
        {
            var found = this.primaryContext.Get<T>(token);

            if (found == null)
            {
                found = this.secondaryContext.Get<T>(token);
            }

            return found;
        }

        /// <summary>
        /// Disposes all items implementing the IDisposable
        /// </summary>
        public void Dispose()
        {
            this.primaryContext.Dispose();
            this.secondaryContext.Dispose();
        }
    }
}