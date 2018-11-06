namespace BurnSystems.Serialization
{
    using System.Collections.Generic;

    /// <summary>
    /// This class stores the value of the array header
    /// </summary>
    public class ArrayHeader
    {
        /// <summary>
        /// List of dimensionsizes
        /// </summary>
        private readonly List<int> _dimensions = new List<int>();

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
        public List<int> Dimensions => _dimensions;
    }
}
