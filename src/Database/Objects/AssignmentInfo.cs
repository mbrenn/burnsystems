using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BurnSystems.Database.Objects
{
    /// <summary>
    /// This class stores information which is used to map between the C#-Type and the databaseobject
    /// </summary>
    internal class AssignmentInfo
    {
        /// <summary>
        /// Gets or sets the name of the column
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Property Info of the associated item
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public Type Type
        {
            get;
            set;
        }

        /// <summary>
        /// Converts an object retrieved from database to an object that shall be stores in the C#-instance
        /// </summary>
        public Func<object, object> ConvertToInstanceProperty
        {
            get;
            set;
        }

        /// <summary>
        /// Converts an object in C#-Instance to an object that shall be stored into database
        /// </summary>
        public Func<object, object> ConvertToDatabaseProperty
        {
            get;
            set;
        }
    }
}
