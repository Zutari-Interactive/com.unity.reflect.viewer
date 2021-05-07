using System;
using System.Data.SqlClient;

namespace Databases.Connection
{
    public static class DatabaseConnection
    {
        #region METHODS

        /// <summary>
        /// Connect To a Database
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="connection"></param>
        public static void ConnectToDatabase(string connectionString, out SqlConnection connection)
        {
            connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Connect to a Database and Invoke an Action.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="callback"></param>
        public static void ConnectToDatabase(string connectionString, Action<SqlConnection> callback)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                callback.Invoke(connection);
            }
        }

        /// <summary>
        /// Connect to a Database and Return a Value from the Database.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ConnectToDatabase<T>(
            string connectionString, Func<SqlConnection, T> callback)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                return callback(connection);
            }
        }

        #endregion
    }
}
