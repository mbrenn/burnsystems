using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Database.Objects
{
    /// <summary>
    /// This attribute indicates that the property shall be used as the primary key for database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DatabaseKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the DatabaseKeyAttribute class.
        /// </summary>
        /// <param name="columnName">Name of the column associated to this property</param>
        public DatabaseKeyAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        /// <summary>
        /// Gets or sets the column name, where the property shall be stored
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }
    }
}
