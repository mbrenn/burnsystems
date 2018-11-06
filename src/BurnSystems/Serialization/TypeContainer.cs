namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Collections;
    using Test;

    /// <summary>
    /// The typecontainer stores and initializes the different type entries
    /// </summary>
    public class TypeContainer
    {
        /// <summary>
        /// Defines the offset for the nonnative types
        /// </summary>
        private const long NonNativeTypeIdOffset = 0x80000000;

        /// <summary>
        /// List of types
        /// </summary>
        private readonly List<TypeEntry> _types = new List<TypeEntry>();

        /// <summary>
        /// Id of the last index
        /// </summary>
        private long _lastIndex = NonNativeTypeIdOffset;

        /// <summary>
        /// Gets the list of types
        /// </summary>
        public List<TypeEntry> Types => _types;

        /// <summary>
        /// Adds a new type
        /// </summary>
        /// <param name="type">Type to be added</param>
        /// <returns>Added TypeEntry</returns>
        public TypeEntry AddType(Type type)
        {
            // Creates new entry
            var typeEntry = new TypeEntry();

            _lastIndex++;
            typeEntry.TypeId = _lastIndex;

            // Sets typeid etc
            typeEntry.Type = type;
            typeEntry.Name = type.FullName;

            if (type.IsGenericType)
            {
                typeEntry.Name = type.GetGenericTypeDefinition().FullName;

                // Adds generic arguments
                foreach (var genericType in type.GetGenericArguments())
                {
                    var genericTypeEntry = FindType(genericType);
                    Ensure.IsNotNull(genericTypeEntry);
                    typeEntry.GenericArguments.Add(genericTypeEntry.TypeId);
                }
            }
            
            // Adds fields     
            var thisType = type;
            while (thisType != null)
            {
                foreach (var field in thisType.GetFields(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    // Checks, if this field already exists
                    if (typeEntry.Fields.Exists(
                        x => 
                            x.FieldInfo.DeclaringType.FullName == field.DeclaringType.FullName
                            && x.FieldInfo.Name == field.Name))
                    {
                        continue;
                    }

                    typeEntry.AddField(field);
                }

                thisType = thisType.BaseType;
            }

            _types.Add(typeEntry);

            return typeEntry;
        }

        /// <summary>
        /// Finds a type with a specific name
        /// </summary>
        /// <param name="name">Requested type name</param>
        /// <returns>Found type</returns>
        public TypeEntry FindType(string name)
        {
            return ListHelper.Find(Types, x => x.Name == name);
        }

        /// <summary>
        /// Finds a type with a specific name
        /// </summary>
        /// <param name="typeId">Id of type</param>
        /// <returns>Found type</returns>
        public TypeEntry FindType(long typeId)
        {
            return ListHelper.Find(Types, x => x.TypeId == typeId);
        }

        /// <summary>
        /// Finds a type with a specific type
        /// </summary>
        /// <param name="type">Requested type</param>
        /// <returns>Found type</returns>
        public TypeEntry FindType(Type type)
        {
            return ListHelper.Find(Types, x => x.Type == type);
        }

        /// <summary>
        /// Adds a typeentry without performing the allocation for a new id
        /// </summary>
        /// <param name="typeEntry">Typeentry to be added</param>
        internal void AddType(TypeEntry typeEntry)
        {
            _types.Add(typeEntry);
        }
    }
}
