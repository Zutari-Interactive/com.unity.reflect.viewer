using UnityEngine;

namespace Databases.ConnectionStrings
{
    [CreateAssetMenu(menuName = "Databases/Connection Strings/SQL Server String")]
    public class SQLServerString : ConnectionString
    {
        #region VARIABLES

        /// <summary>
        /// Use Data Source instead of Server Key.
        /// </summary>
        [Header("String Creation Parameters")]
        public bool UseDataSource = false;

        /// <summary>
        /// Use Initial Catalog instead of Database Key.
        /// </summary>
        public bool UseInitialCatalog = false;


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
        public string UserID = "";

        /// <summary>
        /// User Password
        /// </summary>
        public string Password = "";

        #endregion
    }
}
