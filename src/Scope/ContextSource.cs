using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Test;
using BurnSystems.Logging;

namespace BurnSystems.Scope
{
    /// <summary>
    /// Implementation of <c>IContextSource</c>
    /// </summary>
    public class ContextSource : IContextSource
    {
        /// <summary>
        /// Stores the inner context source
        /// </summary>
        private IContextSource innerContextSource = null;

        /// <summary>
        /// Stores the list of items
        /// </summary>
        private List<Item> items = new List<Item>();

        /// <summary>
        /// Initializes a new instance of the ContextSource class.
        /// </summary>
        public ContextSource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ContextSource class.
        /// </summary>
        /// <param name="innerContextSource">Inner context source that shall be queried, 
        /// if this context source does not find the object</param>
        public ContextSource(IContextSource innerContextSource)
        {
            this.innerContextSource = innerContextSource;
        }

        /// <summary>
        /// Creates a new context
        /// </summary>
        /// <returns>The created context</returns>
        public IContext CreateContext()
        {
            return new Context(this);
        }

        /// <summary>
        /// Adds a factory method which can retrieved within a context
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="factory">Factory method</param>
        public void Add<T>(Func<T> factory)
        {
            Ensure.IsNotNull(factory);

            this.items.Add(
                new Item(() => factory(), typeof(T), string.Empty));
        }

        /// <summary>
        /// Adds a factory method which can retrieved within a context
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="token">Token of the item</param>
        /// <param name="factory">Factory method</param>
        public void Add<T>(string token, Func<T> factory)
        {
            Ensure.IsNotNull(factory);
            if (this.items.Any(x => x.Token.Equals(token)))
            {
                // Already in
                Log.TheLog.LogEntry(
                    LogEntry.Format(LogLevel.Notify, LocalizationBS.Token_Existing, token));
                return;
            }

            this.items.Add(
                new Item(() => factory(), typeof(T), token));
        }

        /// <summary>
        /// Gets the factory method for a type
        /// </summary>
        /// <typeparam name="T">Type to be queried</typeparam>
        /// <returns>Function creating the item</returns>
        public T Create<T>()
        {
            var item = this.items
                .Where(x => x.Type == typeof(T))
                .FirstOrDefault();

            if (item == null)
            {
                if (this.innerContextSource == null)
                {
                    return default(T);
                }

                return this.innerContextSource.Create<T>();
            }

            return (T)item.Value();
        }

        /// <summary>
        /// Gets the factory method for a type
        /// </summary>
        /// <typeparam name="T">Type to be queried</typeparam>
        /// <param name="token">Token to be queries</param>
        /// <returns></returns>
        public T Create<T>(string token)
        {
            var item = this.items
                .Where(x => x.Token == token)
                .FirstOrDefault();

            if (item == null)
            {
                if (this.innerContextSource == null)
                {
                    return default(T);
                }

                return this.innerContextSource.Create<T>(token);
            }

            return (T)item.Value();
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
            /// <param name="type">Type of the item</param>
            /// <param name="token">Token to be added</param>
            public Item(Func<object> value, Type type, string token)
            {
                this.Value = value;
                this.Token = token;
            }

            /// <summary>
            /// Gets or sets the type of the item
            /// </summary>
            public Type Type
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the item
            /// </summary>
            public Func<object> Value
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
