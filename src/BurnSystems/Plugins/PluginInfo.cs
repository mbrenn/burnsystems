﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace BurnSystems.Plugins
{
    /// <summary>
    /// Stores the information about a plugin
    /// </summary>
    public class PluginInfo<T> where T : class
    {
        /// <summary>
        /// Stores a list of dependencies
        /// </summary>
        private readonly List<Type> _dependencies = new List<Type>();

        /// <summary>
        /// Gets or sets the instance of the plugin
        /// </summary>
        public T Instance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the assembly that has been loaded
        /// </summary>
        public Assembly Assembly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type that has been activated
        /// </summary>
        public Type Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of dependencies
        /// </summary>
        public List<Type> Dependencies => _dependencies;

        /// <summary>
        /// Initializes a new instance of the PluginInfo class.
        /// </summary>
        /// <param name="assembly">Assembly of the plugin</param>
        /// <param name="type">Type of the plugin</param>
        /// <param name="instance">Instance of the object that has been created</param>
        public PluginInfo(Assembly assembly, Type type, T instance)
        {
            Assembly = assembly;
            Type = type;
            Instance = instance;
            UpdateDependencies();
        }

        /// <summary>
        /// Updates the dependencies of the plugin
        /// </summary>
        private void UpdateDependencies()
        {
            Dependencies.Clear();

            var attributes = Type.GetCustomAttributes(
                typeof(PluginDependencyAttribute), true);
            foreach (var attributeRaw in attributes)
            {
                if (!(attributeRaw is PluginDependencyAttribute attribute))
                {
                    throw new InvalidOperationException("attribute is null: " +
                                                        (attributeRaw?.ToString() ?? "Unknown"));
                }

                Dependencies.Add(attribute.Type);
            }
        }

        /// <summary>
        /// Converts the plugininfo to a string
        /// </summary>
        /// <returns>Name of the associated type</returns>
        public override string ToString()
        {
            if (Type != null)
            {
                return Type.FullName ?? "Unknown Fullname";
            }

            return "PluginInfo, Unknown Type";
        }
    }
}
