//-----------------------------------------------------------------------
// <copyright file="QueryExtensions.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.AdoNet.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    /// This extension class adds several methods to the query 
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <param name="query">Query to be executed</param>
        /// <returns>Returns the number of affected lines</returns>
        public static int ExecuteNonQuery(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <typeparam name="T">Type of the object that shall be converted between object and dictionary</typeparam>
        /// <param name="connection">Connection to be used</param>
        /// <param name="query">Query to be executed</param>
        /// <returns>Returns the object that has been queried</returns>
        public static T ExecuteScalar<T>(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                return (T)Convert.ChangeType(
                    command.ExecuteScalar(),
                    typeof(T),
                    CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <param name="query">Query to be executed</param>
        /// <returns>Datareader created by the SQL-statement</returns>
        public static IDataReader ExecuteReader(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Executes a query on the given database connection and returns a reader
        /// object with cursor on correct position
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <param name="query">Query to be executed</param>
        /// <returns>Enumeration of datareader</returns>
        public static IEnumerable<IDataReader> ExecuteEnumeration(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader;
                    }
                }
            }
        }
    }
}
