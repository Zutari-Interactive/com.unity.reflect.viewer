using UnityEngine;

namespace Databases.ConnectionStrings
{
    public abstract class ConnectionString : ScriptableObject
    {
        #region VARIABLES

        /// <summary>
        /// Connection String.
        /// </summary>
        [Header("Default String")]
        public string StaticConnectionString = string.Empty;

        /// <summary>
        /// Create a Custom Connection String Flag.
        /// </summary>
        [Header("Options")]
        public bool UseCustomConnectionString = false;

        /// <summary>
        /// Custom Connection String.
        /// </summary>
        [Header("Custom String")]
        public string CustomConnectionString = string.Empty;

        #endregion

        #region METHODS

        /// <summary>
        /// Get Valid Connection String.
        /// </summary>
        /// <returns></returns>
        public string GetValidString()
        {
            if (!string.IsNullOrEmpty(StaticConnectionString) && !string.IsNullOrWhiteSpace(StaticConnectionString))
            {
                return StaticConnectionString;
            }

            if (!string.IsNullOrEmpty(CustomConnectionString) && !string.IsNullOrWhiteSpace(CustomConnectionString))
            {
                return CustomConnectionString;
            }

            return string.Empty;
        }

        #endregion
    }
}
