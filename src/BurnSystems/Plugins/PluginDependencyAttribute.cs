namespace BurnSystems.Plugins
{
    using System;

    /// <summary>
    /// This attribute is used to define the dependency of a plugin 
    /// to another plugin. The Pluginloader has to load the plugins in 
    /// the correct order. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PluginDependencyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the PluginDependencyAttribute class.
        /// </summary>
        /// <param name="type">Parameter to which the type is dependend</param>
        public PluginDependencyAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type to which this plugin is dependend
        /// </summary>
        public Type Type
        {
            get;
            internal set;
        }
    }
}
