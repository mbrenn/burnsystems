namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This class stores the value of the array header
    /// </summary>
    public class ArrayHeader
    {
        /// <summary>
        /// List of dimensionsizes
        /// </summary>
        private List<int> dimensions = new List<int>();

        /// <summary>
        /// Gets or sets the id of type
        /// </summary>
        public long TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id of object
        /// </summary>
        public long ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of dimensions
        /// </summary>
        public int DimensionCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of dimensions
        /// </summary>
        public List<int> Dimensions
        {
            get { return dimensions; }
        }
    }
}
