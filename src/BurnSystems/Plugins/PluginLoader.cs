using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BurnSystems.Logging;

namespace BurnSystems.Plugins
{
    /// <summary>
    /// This class implements a plugin loader for applications. 
    /// It also tries to resolve dependencies within plugins, so they
    /// are loaded in a correct order. 
    /// Plugins can be automatically loaded via AddLocalPluginsWithAttribute
    /// or explicitly added via AddPlugin
    /// </summary>
    public class PluginLoader<T> where T : class
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(PluginLoader<T>));
        /// <summary>
        /// Cache for loaded assemblies
        /// </summary>
        private readonly Dictionary<string, Assembly> _assemblies =
            new Dictionary<string, Assembly>();

        /// <summary>
        /// Stores the list of loaded plugins
        /// </summary>
        private readonly List<PluginInfo<T>> _plugins = new List<PluginInfo<T>>();

        /// <summary>
        /// Gets all plugins
        /// </summary>
        public List<PluginInfo<T>> Plugins => _plugins;

        /// <summary>
        /// Loads all plugins from current directory with a certain attribute
        /// </summary>
        /// <param name="typeAttribute">Type of the attribute</param>
        public void AddLocalPluginsWithAttribute(Type typeAttribute)
        {
            foreach (var assemblyPath in Directory.GetFiles(Environment.CurrentDirectory, "*.dll"))
            {
                try
                {
                    var assembly = EnvironmentHelper.GetOrLoadAssembly(assemblyPath);

                    // Gets all types in assembly, which implement the interface IUmbraPlugin and 
                    // have the UmbraPluginAttribute
                    foreach (var type in assembly.GetTypes()
                        .Where(x => x.GetCustomAttributes(typeAttribute, false).Length > 0)
                        .Where(x => x.GetInterfaces().Any(y => y.FullName == typeof(T).FullName)))
                    {
                        if (!(Activator.CreateInstance(type) is T plugin))
                        {
                            throw new InvalidOperationException("Activator.CreateInstance has returned null");
                        }

                        var pluginInfo = new PluginInfo<T>(assembly, type, plugin);

                        _plugins.Add(pluginInfo);
                    }
                }
                catch (Exception exc)
                {
                    Logger.Error(
                        string.Format(
                            LocalizationBS.PluginLoader_AssemblyNotFound,
                            assemblyPath,
                            exc.Message));
                }
            }
        }

        /// <summary>
        /// Adds a certain plugin with a certain type within a certain assembly to the
        /// local database
        /// </summary>
        /// <param name="assemblyPath">Path tho assembly</param>
        /// <param name="typeName">Name of class, which should be loaded</param>
        public T AddPlugin(string assemblyPath, string typeName)
        {
            try
            {
                // Checks if assembly is in cache
                if (!_assemblies.TryGetValue(assemblyPath, out var assembly))
                {
                    assembly = EnvironmentHelper.GetOrLoadAssembly(assemblyPath);

                    _assemblies[assemblyPath] = assembly;
                }

                // Gets type
                var typeOfPlugin = assembly.GetType(typeName);
                if (typeOfPlugin == null)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            LocalizationBS.PluginLoader_TypeNotFound, typeName, assemblyPath));
                }

                if (!typeof(T).IsAssignableFrom(typeOfPlugin))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            LocalizationBS.PluginLoader_NoPlugin, typeName));
                }

                // Creates the plugin and adds the plugin info
                if (!(Activator.CreateInstance(typeOfPlugin) is T plugin))
                {
                    throw new InvalidOperationException("Created Instance is null");
                }

                var pluginInfo = new PluginInfo<T>(assembly, typeOfPlugin, plugin);

                _plugins.Add(pluginInfo);

                return plugin;
            }
            catch (Exception exc)
            {
                var message = string.Format(
                    LocalizationBS.PluginLoader_FailedToLoadPlugin,
                    assemblyPath,
                    typeName);
                throw new InvalidOperationException(message, exc);
            }
        }

        /// <summary>
        /// Sorts all plugins according to their dependencies
        /// </summary>
        public void SortForDependencies()
        {
            // First sort the plugins
            // Source of order... After sorting, this class has to be empty
            var source = _plugins.ToList();
            Plugins.Clear();

            var hasAdded = false;
            do
            {
                hasAdded = false;
                for (var n = 0; n < source.Count; n++)
                {
                    var element = source[n];
                    if (element.Dependencies.All(
                        x => Plugins.Exists(y => y.Type == x)))
                    {
                        Plugins.Add(element);
                        source.RemoveAt(n);
                        n--;

                        hasAdded = true;
                    }
                }
            }
            while (hasAdded);

            if (source.Count != 0)
            {
                // Some plugins were not added... Dependency problem.
                foreach (var sourceEntry in source)
                {
                    var message = string.Format(
                        LocalizationBS.PluginLoader_InvalidDependency,
                        sourceEntry.Type.FullName);

                    Logger.Fatal(message);

                    foreach (var dependency in sourceEntry.Dependencies)
                    {
                        message = string.Format(
                            "    " + LocalizationBS.PluginLoader_Dependent,
                            dependency.FullName);

                        Logger.Fatal(message);
                    }
                }

                var types = source.Select(x => x.Type.FullName).Aggregate((x, y) => $"{x}, {y}");
                throw new InvalidOperationException("Circular dependencies: " + types);
            }
        }
    }
}
