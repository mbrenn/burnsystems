//-----------------------------------------------------------------------
// <copyright file="ConnectionFactory.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Common;
    using System.Reflection;

    /// <summary>
    /// Enumerates the possible database types that are supported
    /// by the connection factory
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// MySql shall be used
        /// </summary>
        MySql,

        /// <summary>
        /// Sql Server shall be used
        /// </summary>
        SqlServer
    }

    /// <summary>
    /// This static class is a helper class which creates database connections by connection
    /// string and type.
    /// </summary>
    public static class ConnectionFactory
    {
        /// <summary>
        /// Creates a connection string
        /// </summary>
        /// <param name="type">Type of database to be created</param>
        /// <param name="connectionString">Connection string to be used</param>
        /// <returns>Database connection</returns>
        public static DbConnection Create(DatabaseType type, string connectionString)
        {
            if (type == DatabaseType.SqlServer)
            {
                var sqlConnection = new System.Data.SqlClient.SqlConnection(connectionString);
                sqlConnection.Open();

                return sqlConnection;
            }
            else if (type == DatabaseType.MySql)
            {
                Assembly assembly = null;

                try
                {
                    assembly = Assembly.Load("MySql.Data, Version=6.3.6.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d, processorArchitecture=MSIL");
                }
                catch
                {
                    throw new InvalidOperationException("No MySql.Data.dll installed");
                }

                if (assembly == null)
                {
                    throw new InvalidOperationException("No MySql.Data.dll installed");
                }

                var mysqlType = assembly.GetType("MySql.Data.MySqlClient.MySqlConnection");
                if (mysqlType == null)
                {
                    throw new InvalidOperationException("MySql-Type not found");
                }

                var dbConnection = Activator.CreateInstance(mysqlType, new object[] { connectionString }) as DbConnection;
                if (dbConnection == null)
                {
                    throw new InvalidOperationException("MySql Constructor not found");
                }

                dbConnection.Open();

                return dbConnection;
            }
            else
            {
                throw new InvalidOperationException(type.ToString());
            }
        }
    }
}
