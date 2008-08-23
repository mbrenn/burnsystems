﻿//-----------------------------------------------------------------------
// <copyright file="TypeEntry.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BurnSystems.Collections;
    using System.Reflection;

    /// <summary>
    /// This class defines the different types
    /// </summary>
    public class TypeEntry
    {        
        /// <summary>
        /// The methods of this type
        /// </summary>
        List<FieldEntry> fields = new List<FieldEntry>();

        /// <summary>
        /// Id of the last index
        /// </summary>
        int lastIndex;

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
        public Type Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the type
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the methods
        /// </summary>
        public List<FieldEntry> Fields
        {
            get { return this.fields; }
        }

        /// <summary>
        /// Adds a field to this type
        /// </summary>
        /// <param name="field">Field to be added</param>
        public void AddField(FieldInfo field)
        {
            var fieldEntry = new FieldEntry();

            lastIndex++;
            fieldEntry.FieldId = lastIndex;

            fieldEntry.FieldInfo = field;

            fieldEntry.Name = field.Name;
            this.Fields.Add(fieldEntry);
        }

        /// <summary>
        /// Finds a field with a specific id
        /// </summary>
        /// <param name="fieldId">Id of field</param>
        /// <returns>Found field entry or null, if not found</returns>
        public FieldEntry FindField(long fieldId)
        {
            return ListHelper.Find(this.fields, x => x.FieldId == fieldId);
        }

        /// <summary>
        /// Finds a field with a specific name
        /// </summary>
        /// <param name="fieldName">Name of field</param>
        /// <returns>Found field entry or null, if not found</returns>
        public FieldEntry FindField(string fieldName)
        {
            return ListHelper.Find(this.fields, x => x.Name == fieldName);
        }
    }
}
