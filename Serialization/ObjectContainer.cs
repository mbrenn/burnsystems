//-----------------------------------------------------------------------
// <copyright file="ObjectContainer.cs" company="Martin Brenn">
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
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The object container stores the objects, which have already been registered
    /// </summary>
    public class ObjectContainer
    {
        /// <summary>
        /// Stores the object and associates them to a number
        /// </summary>
        Dictionary<object, long> objectToNumber;

        /// <summary>
        /// Stores the number and associates an object
        /// </summary>
        Dictionary<long, object> numberToObject;

        /// <summary>
        /// Last index of the inserted object
        /// </summary>
        long lastIndex;

        /// <summary>
        /// Creates a new container
        /// </summary>
        public ObjectContainer()
        {
            objectToNumber = new Dictionary<object, long>(new ObjectComparer());
            numberToObject = new Dictionary<long, object>();
        }

        /// <summary>
        /// Adds a new object and checks, if the object is already inserted
        /// </summary>
        /// <param name="value">Object to be inserted</param>
        /// <param name="alreadyInserted">Flag, if object has been already inserted</param>
        /// <returns>Id of index</returns>
        public long AddObject(object value, out bool alreadyInserted)
        {
            // Check, if object exists
            long result;
            if (this.objectToNumber.TryGetValue(value, out result))
            {
                alreadyInserted = true;
                return result;
            }

            // Add new entry
            alreadyInserted = false;
            lastIndex++;
            result = lastIndex;
            this.objectToNumber[value] = lastIndex;
            this.numberToObject[lastIndex] = value;

            return result;
        }

        /// <summary>
        /// Adds a new object with a specific object id
        /// </summary>
        /// <param name="objectId">Id of object</param>
        /// <param name="value">Object to be added</param>
        public void AddObject(long objectId, object value)
        {
            this.objectToNumber[value] = objectId;
            this.numberToObject[objectId] = value;
        }

        /// <summary>
        /// Equalitycomparer for references
        /// </summary>
        class ObjectComparer : IEqualityComparer<object>
        {
            #region IEqualityComparer<object> Members

            /// <summary>
            /// Checks, if both objects are equal
            /// </summary>
            /// <param name="x">First object</param>
            /// <param name="y">Second object</param>
            /// <returns>true, if both objects are equal</returns>
            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return object.ReferenceEquals(x, y);
            }

            /// <summary>
            /// Gets the hashcode of the object
            /// </summary>
            /// <param name="obj">Object to be hashed</param>
            /// <returns>Hashcode of object</returns>
            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }

            #endregion
        }

        /// <summary>
        /// Returns an object by objectid
        /// </summary>
        /// <param name="objectId">Id of object</param>
        /// <returns>Found object</returns>
        public object FindObjectById(long objectId)
        {
            return this.numberToObject[objectId];
        }
    }
}
