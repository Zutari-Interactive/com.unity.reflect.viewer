using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Databases.Connection;
using Databases.ConnectionStrings;
using ElementsIOT.General;
using TMPro;
using UnityEngine;

namespace ElementsIOT.IOT.Temperature
{
    public enum HistoricalInterval
    {
        PreviousDay = 2,
        Weekly = 7,
        Fortnitely = 14,
    }

    public class TemperatureConnection : MonoBehaviour
    {
        #region VARIABLES

        [Header("Connection String")]
        public SQLServerString ConnectionString;

        [Header("Temperature Tag")]
        public string Tag = "3LIT01_ANALOG.value";

        [Header("Historical Data Interval")]
        public HistoricalInterval HistoricalInterval = HistoricalInterval.PreviousDay;

        [Header("Records")]
        public string LatestRecord = string.Empty;
        public List<string> HistoricalRecords = new List<string>();

        private SqlConnection _cachedConnection = null;
        private string _latestQuery = @"SELECT TOP 1 * FROM test WHERE Tagname = @tagname ORDER BY dt DESC";
        private string _historicalQuery =
            @"SELECT TOP (@taginterval) * FROM test WHERE Tagname = @tagname ORDER BY dt DESC";

        #endregion


        #region METHODS

        private void EstablishConnection()
        {
            if (_cachedConnection != null) return;
            print("Establishing Database Connection!");
            DatabaseConnection.ConnectToDatabase(ConnectionString.GetValidString(), out _cachedConnection);
        }

        public DataTable GetLatestRecord()
        {
            EstablishConnection();
            _cachedConnection.Open();

            DataTable table = null;
            using (SqlCommand command = new SqlCommand(_latestQuery, _cachedConnection))
            {
                command.Parameters.AddWithValue("@tagname", Tag);
                print("Creating SQL Command.");
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    print("Processing SQL Command.");
                    table = new DataTable();
                    adapter.Fill(table);
                }

                print("Processing SQL Command Completed!");
            }

            _cachedConnection.Close();
            return table;
        }

        public DataTable GetHistoricalRecords()
        {
            EstablishConnection();
            _cachedConnection.Open();

            DataTable table = null;
            using (SqlCommand command = new SqlCommand(_historicalQuery, _cachedConnection))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@tagname", SqlDbType.Char) {Value = Tag},
                    new SqlParameter("@taginterval", SqlDbType.Int)
                        {Value = (int) HistoricalInterval * Constants.HalfHoursInADay},
                };

                command.Parameters.AddRange(parameters.ToArray());

                print("Creating SQL Command.");
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    print("Processing SQL Command.");
                    table = new DataTable();
                    adapter.Fill(table);
                }

                print("Processing SQL Command Completed!");
            }

            _cachedConnection.Close();
            return table;
        }

        public void UpdateLatestRecord(TMP_Text text)
        {
            DataTable table = GetLatestRecord();
            LatestRecord = $"{table.Rows[0].ItemArray[3]}";
            text.SetText(LatestRecord);
        }

        public void UpdateHistoricalRecords()
        {
            DataTable table = GetHistoricalRecords();
            HistoricalRecords.Clear();
            int length = table.Rows.Count;
            int index = 0;

            while (index < length)
            {
                HistoricalRecords.Add($"{table.Rows[index].ItemArray[3]}");
                index++;
            }
        }

        #endregion
    }
}
