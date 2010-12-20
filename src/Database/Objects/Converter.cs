using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Database.Objects
{
    /// <summary>
    /// This class maps the C# instance type to a database table and offers methods
    /// the insert, update, delete or get items. 
    /// </summary>
    public class Converter<T> where T : new()
    {
        /// <summary>
        /// Stores the assignments 
        /// </summary>
        private static List<AssignmentInfo> assignments = new List<AssignmentInfo>();

        /// <summary>
        /// Stores the assignments  without key
        /// </summary>
        private static List<AssignmentInfo> assignmentsWithoutKey = new List<AssignmentInfo>();

        /// <summary>
        /// Stores the information about the primary key
        /// </summary>
        private static AssignmentInfo primaryKey = null;

        /// <summary>
        /// Gets or sets the information about the primary key
        /// </summary>
        private static AssignmentInfo PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }

        /// <summary>
        /// Gets the columnname of the primary key
        /// </summary>
        public string PrimaryKeyName
        {
            get { return primaryKey.ColumnName; }
        }

        /// <summary>
        /// Static constructor which is used to update internal table about
        /// properties
        /// </summary>
        static Converter()
        {
            var type = typeof(T);

            // First check, if this type has an attribute DatabaseClassAttribute
            var databaseClassAttribute = type.GetCustomAttributes(typeof(DatabaseClassAttribute), false).FirstOrDefault();
            if (databaseClassAttribute == null)
            {
                throw new InvalidOperationException(LocalizationBS.Mapper_AttributeDatabaseClassNotSet);
            }

            // Create array of databaseproperties
            var properties = type.GetProperties(System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in properties)
            {
                var databaseKeyAttribute = property.GetCustomAttributes(typeof(DatabaseKeyAttribute), false).FirstOrDefault() as DatabaseKeyAttribute;
                var databasePropertyAttribute = property.GetCustomAttributes(typeof(DatabasePropertyAttribute), false).FirstOrDefault() as DatabasePropertyAttribute;

                if (databaseKeyAttribute != null && databasePropertyAttribute != null)
                {
                    throw new InvalidOperationException(LocalizationBS.Mapper_KeyAndPropertyAttributeSet);
                }

                // If Key of class is found
                if (databaseKeyAttribute != null)
                {
                    if (PrimaryKey != null)
                    {
                        throw new InvalidOperationException(LocalizationBS.Mapper_MultipleDatabaseKeyAttribute);
                    }
                    
                    PrimaryKey = ConvertPropertyInfo(databaseKeyAttribute.ColumnName, property);

                    // Check if databasekey is correct
                    if (PrimaryKey.Type != typeof(System.Int32) && PrimaryKey.Type != typeof(System.Int64))
                    {
                        throw new InvalidOperationException(LocalizationBS.Mapper_WrongTypeForDatabaseKeyAttribute);
                    }

                    assignments.Add(PrimaryKey);
                }

                // If this property shall be stored into database
                if (databasePropertyAttribute != null)
                {
                    var assignment = ConvertPropertyInfo(databasePropertyAttribute.ColumnName, property);
                    assignments.Add(assignment);
                    assignmentsWithoutKey.Add(assignment);
                }
            }

            if (PrimaryKey == null)
            {
                throw new InvalidOperationException(LocalizationBS.Mapper_NoDatabaseKeyAttribute);
            }
        }

        /// <summary>
        /// Converts the instance to a databaseobject
        /// </summary>
        /// <param name="item">Instance to be converted</param>
        /// <param name="includingPrimaryKey">true, if the primary key shall also be converted</param>
        /// <returns>Dictionary of properties of the item</returns>
        public Dictionary<string, object> ConvertToDatabaseObject(T item, bool includingPrimaryKey)
        {
            var result = new Dictionary<string, object>();
            
            var list = includingPrimaryKey ? assignments : assignmentsWithoutKey;

            foreach (var pair in list)
            {
                var rawValue = pair.PropertyInfo.GetGetMethod().Invoke(item, null);
                var databaseValue = pair.ConvertToDatabaseProperty(rawValue);
                result[pair.ColumnName] = databaseValue;
            }

            return result;
        }

        /// <summary>
        /// Converts the instance to a databaseobject
        /// </summary>
        /// <param name="item">Instance to be converted</param>
        /// <returns>Dictionary of properties of the item</returns>
        public T ConvertToInstance(Dictionary<string, object> item)
        {
            var result = new T();

            foreach (var pair in assignments)
            {
                var rawValue = item[pair.ColumnName];
                var instanceValue = pair.ConvertToInstanceProperty(rawValue);
                pair.PropertyInfo.GetSetMethod().Invoke(result, new object[] { instanceValue });
            }

            return result;
        }

        /// <summary>
        /// Gets the primary key id of the given object
        /// </summary>
        /// <param name="t">Object, whose primary key is required</param>
        /// <returns>Id of the object</returns>
        public long GetId(T t)
        {
            var rawValue = primaryKey.PropertyInfo.GetGetMethod().Invoke(t, null);
            var value = primaryKey.ConvertToDatabaseProperty(rawValue);
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// Sets the primary key of the given object
        /// </summary>
        /// <param name="t">Object, whose primary key shall be set</param>
        /// <param name="id">Id of the object</param>
        public void SetId(T t, long id)
        {
            var value = primaryKey.ConvertToInstanceProperty(id);
            primaryKey.PropertyInfo.GetSetMethod().Invoke(t, new object[] { value });
        }

        /// <summary>
        /// Converts the PropertyInfo returned by reflection to an AssignmentInfo class
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="property">PropertyInfo to be converted</param>
        /// <returns>Converted PropertyInfo</returns>
        private static AssignmentInfo ConvertPropertyInfo(string columnName, System.Reflection.PropertyInfo property)
        {
            var info = new AssignmentInfo();
            info.ColumnName = columnName;
            info.PropertyInfo = property;
            info.Type = property.PropertyType;
            info.ConvertToDatabaseProperty = (x) => x;

            if (info.Type == typeof(string))
            {
                info.ConvertToInstanceProperty = (x) => x.ToString();
            }
            else if (info.Type == typeof(System.Int16))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToInt16(x);
            }
            else if (info.Type == typeof(System.Int32))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToInt32(x);
            }
            else if (info.Type == typeof(System.Int64))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToInt64(x);
            }
            else if (info.Type == typeof(System.Single))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToSingle(x);
            }
            else if (info.Type == typeof(System.Double))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToDouble(x);
            }
            else if (info.Type == typeof(System.Decimal))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToDecimal(x);
            }
            else if (info.Type == typeof(System.DateTime))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToDateTime(x);
            }
            else if (info.Type.IsEnum)
            {
                info.ConvertToDatabaseProperty = (x) => x.ToString();
                info.ConvertToInstanceProperty = (x) =>
                    {
                        return Enum.Parse(info.Type, x.ToString());
                    };
            }
            else
            {
                throw new InvalidOperationException(string.Format(LocalizationBS.Mapper_NotSupportedType, info.Type.ToString()));
            }

            return info;
        }
    }
}
