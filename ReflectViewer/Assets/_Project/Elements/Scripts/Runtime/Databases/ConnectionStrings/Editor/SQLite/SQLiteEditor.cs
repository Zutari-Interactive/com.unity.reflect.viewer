#if UNITY_EDITOR
using UnityEditor;

namespace Databases.ConnectionStrings
{
    [CustomEditor(typeof(SQLiteString))]
    public class SQLiteEditor : Editor
    {
        #region VARIABLES

        /// <summary>
        /// SQLite String Instance.
        /// </summary>
        private SQLiteString _target;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _target = target as SQLiteString;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        #endregion

        #region METHODS

        #endregion
    }
}
#endif
