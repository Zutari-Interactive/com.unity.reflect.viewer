using UnityEngine;

namespace Databases.ConnectionStrings
{
    [CreateAssetMenu(menuName = "Databases/Connection Strings/MySQL String")]
    public class MySQLString : ConnectionString
    {
        #region VARIABLES

        /// <summary>
        /// Use an Encrypted Connection.
        /// </summary>
        [Header("String Creation Parameters")]
        public bool UseEncryption = false;

        /// <summary>
        /// Use Windows Authentication Flag.
        /// </summary>
        public bool UseWindowsAuthentication = false;

        /// <summary>
        /// Pool Connection to Database.
        /// </summary>
        public bool ConnectionPooling = false;

        /// <summary>
        /// Server Address
        /// </summary>
        [Header("General")]
        public string Server = "";

        /// <summary>
        /// Database Name
        /// </summary>
        public string Database = "";

        /// <summary>
        /// User ID
        /// </summary>
        public string Uid = "";

        /// <summary>
        /// User Password
        /// </summary>
        public string Pwd = "";

        /// <summary>
        /// Use Encryption.
        /// </summary>
        public string SslMode = "Preferred";

        /// <summary>
        /// Enable Connection Pool
        /// </summary>
        [Header("Additional Properties")]
        public bool Pooling = true;

        /// <summary>
        /// Minimum Pool Size
        /// </summary>
        public int MinimumPoolSize = 10;

        /// <summary>
        /// Maximum Pool Size
        /// </summary>
        public int MaximumPoolSize = 50;

        /// <summary>
        /// Reset Connection State.
        /// </summary>
        public bool ConnectionReset = true;

        /// <summary>
        /// Connection Life Time
        /// </summary>
        public int ConnectionLifeTime = 300;

        /// <summary>
        /// Cache Servers Properties.
        /// </summary>
        public bool CacheServerProperties = true;

        #endregion
    }
}
