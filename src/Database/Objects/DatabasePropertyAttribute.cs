using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Database.Objects
{
    /// <summary>
    /// This attributes indicates that the property shall be stored into database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DatabasePropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the DatabasePropertyAttribute class.
        /// </summary>
        /// <param name="columnName">Name of the column associated to this property</param>
        public DatabasePropertyAttribute(string columnName)
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
