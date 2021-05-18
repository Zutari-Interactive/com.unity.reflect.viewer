#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;


namespace Databases.ConnectionStrings
{
    [CustomEditor(typeof(SQLServerString))]
    public class SQLServerStringEditor : Editor
    {
        #region VARIABLES

        /// <summary>
        /// SQLServer String Instance.
        /// </summary>
        private SQLServerString _target;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Use Custom String Flag.
        /// </summary>
        private bool UseCustomString
        {
            get => _target.UseCustomConnectionString;
            set => _target.UseCustomConnectionString = value;
        }

        /// <summary>
        /// Static Connection String.
        /// </summary>
        private string StaticConnectionString
        {
            get => _target.StaticConnectionString;
            set => _target.StaticConnectionString = value;
        }

        /// <summary>
        /// Custom Connection String.
        /// </summary>
        private string CustomConnectionString
        {
            get => _target.CustomConnectionString;
            set => _target.CustomConnectionString = value;
        }

        private bool UseDataSource
        {
            get => _target.UseDataSource;
            set => _target.UseDataSource = value;
        }

        private bool UseInitialCatalog
        {
            get => _target.UseInitialCatalog;
            set => _target.UseInitialCatalog = value;
        }

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _target = target as SQLServerString;
        }

        public override void OnInspectorGUI()
        {
            Default(15);

            if (!UseCustomString)
            {
                ConnectionStringParameters(15);
                ConnectionStringFields(15);
            }

            EditStaticString();
            SaveString();
        }

        #endregion

        #region METHODS

        private void Default(float space = 0f)
        {
            UseCustomString = EditorGUILayout.ToggleLeft("Use Custom String", UseCustomString);
            EditorGUILayout.Space();

            if (UseCustomString)
            {
                EditorGUILayout.LabelField("Custom Connection String");
                CustomConnectionString =
                    EditorGUILayout.TextArea(CustomConnectionString, EditorStyles.helpBox, GUILayout.Height(96f));
            }
            else
            {
                EditorGUILayout.LabelField("Static Connection String");
                EditorGUILayout.TextArea(StaticConnectionString, EditorStyles.helpBox, GUILayout.Height(96f));
            }

            GUILayout.Space(space);
        }

        private void ConnectionStringParameters(float space = 0f)
        {
            EditorGUILayout.LabelField("Connection String Parameters");

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            UseDataSource = EditorGUILayout.ToggleLeft("Use DataSource", UseDataSource, GUILayout.Width(96f),
                                                       GUILayout.ExpandWidth(true));

            UseInitialCatalog = EditorGUILayout.ToggleLeft("Use Initial Catalog", UseInitialCatalog,
                                                           GUILayout.Width(96f), GUILayout.ExpandWidth(true));

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(space);
        }

        private void ConnectionStringFields(float space = 0f)
        {
            EditorGUILayout.LabelField("Connection String Fields");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _target.Server = EditorGUILayout.TextField(UseDataSource ? "DataSource:" : "Server:", _target.Server,
                                                       EditorStyles.helpBox);

            _target.Database = EditorGUILayout.TextField(UseInitialCatalog ? "Initial Catalog:" : "Database:",
                                                         _target.Database, EditorStyles.helpBox);

            _target.UserID = EditorGUILayout.TextField("Username:", _target.UserID, EditorStyles.helpBox);
            _target.Password = EditorGUILayout.TextField("Password:", _target.Password, EditorStyles.helpBox);

            EditorGUILayout.EndVertical();
            GUILayout.Space(space);
        }

        private void EditStaticString(float space = 0f)
        {
            string connectionString = String.Empty;

            if (UseDataSource)
            {
                connectionString += $"Datasource={_target.Server};";
            }
            else
            {
                connectionString += $"Server={_target.Server};";
            }

            if (UseInitialCatalog)
            {
                connectionString += $"Initial Catalog={_target.Database};";
            }
            else
            {
                connectionString += $"Database={_target.Database};";
            }

            connectionString += $"User ID={_target.UserID};";
            connectionString += $"Password={_target.Password};";
            StaticConnectionString = connectionString;

            GUILayout.Space(space);
        }

        private void SaveString()
        {
            if (GUILayout.Button("Save Connection String"))
            {
                EditorUtility.SetDirty(_target);
            }
        }

        #endregion
    }
}
#endif
