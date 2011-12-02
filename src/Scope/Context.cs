using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Test;
using BurnSystems.Logging;

namespace BurnSystems.Scope
{
    /// <summary>
    /// The context is the implementation of the <c>IContext</c> class. 
    /// </summary>
    public class Context : IContext
    {
        /// <summary>
        /// Stores the source
        /// </summary>
        private List<IContextSource> sources = new List<IContextSource>();

        /// <summary>
        /// Stores the list of items
        /// </summary>
        private List<Item> items = new List<Item>();

        /// <summary>
        /// Gets the local context source which can be used to add additional factory methods
        /// </summary>
        public IContextSource LocalContextSource
        {
            get
            {
                return this.sources.First();
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the Context class
        /// </summary>
        /// <param name="source">Source of the context</param>
        public Context(IContextSource source)
        {
            this.sources.Add(new ContextSource());
            Ensure.IsNotNull(source);
            this.sources.Add(source);
        }

        /// <summary>
        /// Adds an item to context
        /// </summary>
        /// <param name="source">Source to be added</param>
        public void Add(IContextSource source)
        {
            Ensure.IsNotNull(source);
            
            if(this.sources.Any(x=>x.Equals(source)))
            {
                // Already in
                return;
            }
            
            this.sources.Add(source);
        }

        /// <summary>
        /// Tries to add the context source of the given object. 
        /// If the object implements IContextSource or IContextSourceFactory, the object
        /// can be added
        /// </summary>
        /// <param name="value">Object perhaps containing a context source</param>
        /// <returns>true, if something could be added, otherwise false</returns>
        public bool TryToAddContextSource(object value)
        {
            var contextSource = value as IContextSource;
            if (contextSource != null)
            {
                this.Add(contextSource);
                return true;
            }

            var contextSourceFactory = value as IContextSourceFactory;
            if (contextSourceFactory != null)
            {
                this.Add(contextSourceFactory);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a contextsource created by the factory
        /// </summary>
        /// <param name="sourceFactory">Sourcefactory to be used</param>
        public void Add(IContextSourceFactory sourceFactory)
        {
            Ensure.IsNotNull(sourceFactory);
            this.Add(sourceFactory.CreateContextSource());
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
            var found = 
                this.items
                    .Where(x => x.Value.GetType().Equals(typeof(T)))
                    .Cast<T>()
                    .FirstOrDefault();

            if (found == null)
            {
                found = this.sources
                    .Select(x=>x.Create<T>())
                    .Where(x=>x != null)
                    .FirstOrDefault();
                
                if(found != null)
                {
                    this.items.Add(
                        new Item(found, string.Empty));
                }
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
            var found = 
                this.items
                    .Where(x => x.Token == token)
                    .Cast<T>()
                    .FirstOrDefault();

            if (found == null)
            {
                found = this.sources
                    .Select(x=>x.Create<T>(token))
                    .Where(x=>x != null)
                    .FirstOrDefault();
                
                if(found != null)
                {
                    this.items.Add(
                        new Item(found, token));
                }
            }

            return found;
        }

        /// <summary>
        /// Disposes all items implementing the IDisposable
        /// </summary>
        public void Dispose()
        {
            foreach (var item in this.items)
            {
                var disposable = item.Value as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            this.items.Clear();
        }

        /// <summary>
        /// Stores the information
        /// </summary>
        private class Item
        {
            /// <summary>
            /// Initializes a new instance of the Item class.
            /// </summary>
            /// <param name="value">Item to be added</param>
            /// <param name="token">Token to be added</param>
            public Item(object value, string token)
            {
                this.Value = value;
                this.Token = token;
            }

            /// <summary>
            /// Gets or sets the item
            /// </summary>
            public object Value
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the token
            /// </summary>
            public string Token
            {
                get;
                set;
            }
        }
    }
}
