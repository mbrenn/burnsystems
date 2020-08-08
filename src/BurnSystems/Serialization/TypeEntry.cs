using System;
using System.Collections.Generic;
using System.Reflection;
using BurnSystems.Collections;

namespace BurnSystems.Serialization
{
    /// <summary>
    /// This class defines the different types
    /// </summary>
    public class TypeEntry
    {
        /// <summary>
        /// Id of the last index
        /// </summary>
        private int _lastIndex;

        /// <summary>
        /// Gets or sets the id of the type
        /// </summary>
        public long TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the associated type
        /// </summary>
        public Type? Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the type
        /// </summary>
        public string? Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the methods
        /// </summary>
        public List<FieldEntry> Fields { get; } = new List<FieldEntry>();

        /// <summary>
        /// Gets the generic arguments
        /// </summary>
        public List<long> GenericArguments { get; } = new List<long>();

        /// <summary>
        /// Adds a field to this type
        /// </summary>
        /// <param name="field">Field to be added</param>
        public void AddField(FieldInfo field)
        {
            var fieldEntry = new FieldEntry();

            _lastIndex++;
            fieldEntry.FieldId = _lastIndex;

            fieldEntry.FieldInfo = field;

            fieldEntry.Name = field.Name;
            Fields.Add(fieldEntry);
        }

        /// <summary>
        /// Finds a field with a specific id
        /// </summary>
        /// <param name="fieldId">Id of field</param>
        /// <returns>Found field entry or null, if not found</returns>
        public FieldEntry FindField(long fieldId)
        {
            return ListHelper.Find(Fields, x => x.FieldId == fieldId);
        }

        /// <summary>
        /// Finds a field with a specific name
        /// </summary>
        /// <param name="fieldName">Name of field</param>
        /// <returns>Found field entry or null, if not found</returns>
        public FieldEntry FindField(string fieldName)
        {
            return ListHelper.Find(Fields, x => x.Name == fieldName);
        }

        /// <summary>
        /// Converts this instance to string
        /// </summary>
        /// <returns>Name of type</returns>
        public override string ToString()
        {
            return Name ?? "None";
        }
    }
}
