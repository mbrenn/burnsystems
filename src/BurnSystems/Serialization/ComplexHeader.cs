namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This header stores the data for a complex structure
    /// </summary>
    public class ComplexHeader
    {
        /// <summary>
        /// Gets or sets the id of the type
        /// </summary>
        public long TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object id
        /// </summary>
        public long ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of properties
        /// </summary>
        public int FieldCount
        {
            get;
            set;
        }
    }
}
