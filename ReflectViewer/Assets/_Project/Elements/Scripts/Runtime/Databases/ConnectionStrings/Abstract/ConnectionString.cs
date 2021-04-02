using UnityEngine;

namespace Databases.ConnectionStrings
{
    public abstract class ConnectionString : ScriptableObject
    {
        #region VARIABLES

        /// <summary>
        /// Connection String.
        /// </summary>
        [Header("Default")]
        public string StaticConnectionString = string.Empty;

        /// <summary>
        /// Create a Custom Connection String Flag.
        /// </summary>
        public bool UseCustomConnectionString = false;

        /// <summary>
        /// Custom Connection String.
        /// </summary>
        public string CustomConnectionString = string.Empty;

        #endregion

        #region METHODS

        /// <summary>
        /// Get Valid Connection String.
        /// </summary>
        /// <returns></returns>
        public string GetValidString()
        {
            if (StaticConnectionString != string.Empty && !string.IsNullOrEmpty(StaticConnectionString) &&
                !string.IsNullOrWhiteSpace(StaticConnectionString))
            {
                return StaticConnectionString;
            }

            if (CustomConnectionString != string.Empty && !string.IsNullOrEmpty(CustomConnectionString) &&
                !string.IsNullOrWhiteSpace(CustomConnectionString))
            {
                return CustomConnectionString;
            }

            return "";
        }

        #endregion
    }
}
