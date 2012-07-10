using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// When automatic binding is used, this attribute hints that the ByName binding shall be used. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ByNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the binding
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the ByNameAttribute class.
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        public ByNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
