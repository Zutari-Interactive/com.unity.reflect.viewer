using System;
using System.Data;
using System.Data.SqlClient;
using Databases.Connection;

namespace Databases.Query
{
    public static class Query
    {
        /// <summary>
        /// Read the Database Only.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static void ReadDatabaseOnly(string connectionString, string query, Action<SqlDataReader> callback)
        {
            DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          callback.Invoke(command.ExecuteReader());
                                      }
                                  });
        }


        /// <summary>
        /// Read a Database using a Parameterized Query.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="callback"></param>
        /// <param name="parameters"></param>
        public static void ReadDatabaseOnly(
            string connectionString, string query, Action<SqlCommand> callback)
        {
            DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          callback.Invoke(command);
                                      }
                                  });
        }

        /// <summary>
        /// Store a Single Table of a Database in Memory.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable ReturnDataTable(string connectionString, string query)
        {
            return DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                          {
                                              DataTable dataTable = new DataTable();
                                              adapter.Fill(dataTable);

                                              return dataTable;
                                          }
                                      }
                                  });
        }

        /// <summary>
        /// Store a Single Table of a Database using a Parameterized Query.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="callback"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ReturnDataTable(
            string connectionString, string query, Action<SqlCommand> callback)
        {
            return DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          // ToDo : Parameterized Query using SqlCommand Instance.
                                          callback.Invoke(command);

                                          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                          {
                                              DataTable table = new DataTable();
                                              adapter.Fill(table);

                                              return table;
                                          }
                                      }
                                  });
        }

        /// <summary>
        /// Get a Reference to a DataTable.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="table"></param>
        public static void FillDataTable(string connectionString, string query, out DataTable table)
        {
            table = DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                          {
                                              DataTable dataTable = new DataTable();
                                              adapter.Fill(dataTable);

                                              return dataTable;
                                          }
                                      }
                                  });
        }

        /// <summary>
        /// Out a Single DataTable using a Parameterized Query.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="callback"></param>
        /// <param name="table"></param>
        /// <param name="parameters"></param>
        public static void FillDataTable(
            string connectionString, string query, Action<SqlCommand> callback, out DataTable table)
        {
            table = DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          callback.Invoke(command);

                                          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                          {
                                              DataTable dataTable = new DataTable();
                                              adapter.Fill(dataTable);

                                              return dataTable;
                                          }
                                      }
                                  });
        }

        /// <summary>
        /// Store DataTables in a Dataset.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet ReturnDataSet(string connectionString, string query)
        {
            return DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                          {
                                              DataSet set = new DataSet();
                                              adapter.Fill(set);

                                              return set;
                                          }
                                      }
                                  });
        }

        /// <summary>
        /// Store DataTables in a Dataset using a Parameterized Query.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="callback"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataSet ReturnDataSet(
            string connectionString, string query, Action<SqlCommand> callback)
        {
            return DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          callback.Invoke(command);

                                          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                          {
                                              DataSet set = new DataSet();
                                              adapter.Fill(set);

                                              return set;
                                          }
                                      }
                                  });
        }

        /// <summary>
        /// Store DataTables in a DataSet.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="set"></param>
        public static void FillDataSet(string connectionString, string query, out DataSet set)
        {
            set = DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                          {
                                              DataSet dataSet = new DataSet();
                                              adapter.Fill(dataSet);

                                              return dataSet;
                                          }
                                      }
                                  });
        }

        /// <summary>
        /// Store Data Tables in a DataSet using a Parameterized Query.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <param name="callback"></param>
        /// <param name="set"></param>
        /// <param name="parameters"></param>
        public static void FillDataSet(
            string connectionString, string query, Action<SqlCommand> callback, out DataSet set)
        {
            set = DatabaseConnection.ConnectToDatabase(
                connectionString, (connection) =>
                                  {
                                      using (SqlCommand command = new SqlCommand(query, connection))
                                      {
                                          callback.Invoke(command);

                                          using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                          {
                                              DataSet dataSet = new DataSet();
                                              adapter.Fill(dataSet);

                                              return dataSet;
                                          }
                                      }
                                  });
        }
    }
}