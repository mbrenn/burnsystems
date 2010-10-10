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

    /// <summary>
    /// This extension class adds several methods to the query 
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <param name="query">Query to be executed</param>
        /// <param name="connection">Connection to be used</param>
        public static void ExecuteNonQuery(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <param name="query">Query to be executed</param>
        /// <param name="connection">Connection to be used</param>
        public static T ExecuteScalar<T>(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                return (T) Convert.ChangeType(command.ExecuteScalar(), typeof(T));
            }
        }

        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <param name="query">Query to be executed</param>
        /// <param name="connection">Connection to be used</param>
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
        /// <param name="query">Query to be executed</param>
        /// <param name="connection">Connection to be used</param>
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
