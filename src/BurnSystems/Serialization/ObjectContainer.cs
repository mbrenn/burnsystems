﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BurnSystems.Serialization
{
    /// <summary>
    /// The object container stores the objects, which have already been registered
    /// </summary>
    internal class ObjectContainer
    {
        /// <summary>
        /// Stores the object and associates them to a number
        /// </summary>
        private readonly Dictionary<object, long> _objectToNumber;

        /// <summary>
        /// Stores the number and associates an object
        /// </summary>
        private readonly Dictionary<long, object> _numberToObject;

        /// <summary>
        /// Last index of the inserted object
        /// </summary>
        private long _lastIndex;

        /// <summary>
        /// Initializes a new instance of the ObjectContainer class.
        /// </summary>
        public ObjectContainer()
        {
            _objectToNumber = new Dictionary<object, long>(new ObjectComparer());
            _numberToObject = new Dictionary<long, object>();
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
            if (_objectToNumber.TryGetValue(value, out var result))
            {
                alreadyInserted = true;
                return result;
            }

            // Add new entry
            alreadyInserted = false;
            _lastIndex++;
            result = _lastIndex;
            _objectToNumber[value] = _lastIndex;
            _numberToObject[_lastIndex] = value;

            return result;
        }

        /// <summary>
        /// Adds a new object with a specific object id
        /// </summary>
        /// <param name="objectId">Id of object</param>
        /// <param name="value">Object to be added</param>
        public void AddObject(long objectId, object value)
        {
            _objectToNumber[value] = objectId;
            _numberToObject[objectId] = value;
        }

        /// <summary>
        /// Returns an object by objectid
        /// </summary>
        /// <param name="objectId">Id of object</param>
        /// <returns>Found object</returns>
        public object FindObjectById(long objectId)
        {
            return _numberToObject[objectId];
        }

        /// <summary>
        /// Equalitycomparer for references
        /// </summary>
        private class ObjectComparer : IEqualityComparer<object>
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
                return ReferenceEquals(x, y);
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
    }
}
