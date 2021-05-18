using UnityEngine;

namespace Databases.ConnectionStrings
{
    [CreateAssetMenu(menuName = "Databases/Connection Strings/SQLite String")]
    public class SQLiteString : ConnectionString
    {
        #region VARIABLES

        /// <summary>
        /// SQLite Database File Location
        /// </summary>
        [Header("General")]
        public string DataSource;

        /// <summary>
        /// Version of the Database
        /// </summary>
        public int Version = 3;

        /// <summary>
        /// Specify the UTF 16 Encoding
        /// </summary>
        public bool UseUTF16Encoding = true;

        /// <summary>
        /// Password
        /// </summary>
        public string Password = "";

        /// <summary>
        /// Use Pooling
        /// </summary>
        public bool Pooling = true;

        /// <summary>
        /// Max Size of the Pool
        /// </summary>
        public int MaxPoolSize = 100;

        /// <summary>
        /// Database Readonly Flag.
        /// </summary>
        [Header("Additional Properties")]
        public bool ReadOnly = true;

        /// <summary>
        /// Date Time As Ticks
        /// </summary>
        public string DateTimeAsTicks = @"Ticks";

        /// <summary>
        /// Binary GUID
        /// </summary>
        public bool BinaryGUID = false;

        /// <summary>
        /// Cache Size of Database
        /// </summary>
        public int CacheSize = 2000;

        /// <summary>
        /// Page Size of Database.
        /// </summary>
        public int PageSize = 1024;

        /// <summary>
        /// Max Page Count.
        /// </summary>
        public int MaxPageCount = 5000;

        #endregion
    }
}
