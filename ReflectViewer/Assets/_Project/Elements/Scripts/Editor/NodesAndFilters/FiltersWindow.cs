#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Elements.Editors
{
    public class FiltersWindow : EditorWindow
    {
        #region WINDOW

        public static FiltersWindow Window;

        [MenuItem("Elements/Create/Custom Filter")]
        public static void OpenWindow()
        {
            Window = GetWindow<FiltersWindow>("Filter Window");
            Window.Show();
        }

        #endregion

        #region VARIABLES

        private string _namespace             = string.Empty;
        private string _customFilterClassName = string.Empty;
        private string _reflectNode           = string.Empty;
        private string _processFilter         = string.Empty;

        private TextAsset _customFilterTemplate;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            if (EditorPrefs.HasKey("$NAMESPACE")) _namespace = EditorPrefs.GetString("$NAMESPACE");
            _customFilterTemplate = Resources.Load<TextAsset>("template-custom-filter");
        }

        public void OnGUI()
        {
            NamespaceField();
            ClassNameFields();
            VariableFields();
            ButtonField();
        }

        #endregion

        #region METHODS

        private void NamespaceField()
        {
            EditorGUI.BeginChangeCheck();

            _namespace = EditorGUILayout.TextField("Namespace:", _namespace);

            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString("$NAMESPACE", _namespace);
            }
        }

        private void ClassNameFields()
        {
            _customFilterClassName = EditorGUILayout.TextField("Custom Filter Class Name:", _customFilterClassName);
            EditorGUILayout.Space();
        }

        private void VariableFields()
        {
            _reflectNode = EditorGUILayout.TextField("Reflect Node:", _reflectNode);
            _processFilter = EditorGUILayout.TextField("Reflect Node Processor:", _processFilter);
            EditorGUILayout.Space();
        }

        private void ButtonField()
        {
            if (GUILayout.Button("Compile Node"))
            {
                if (string.IsNullOrEmpty(_customFilterClassName) || string.IsNullOrEmpty(_reflectNode) ||
                    string.IsNullOrEmpty(_processFilter)         || string.IsNullOrEmpty(_namespace))
                {
                    Debug.LogError("You have Missing Information!");
                    return;
                }

                ReplaceKeysInData(out string data);
                string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, _customFilterClassName, "cs");
                File.WriteAllText(path, data);
                AssetDatabase.Refresh();
            }
        }

        private void ReplaceKeysInData(out string data)
        {
            data = _customFilterTemplate.text.Replace("$NAMESPACE", _namespace);
            data = data.Replace("$CUSTOMFILTERNAME", _customFilterClassName);
            data = data.Replace("$REFLECTNODE", _reflectNode);
            data = data.Replace("$IREFLECTPROCESSORNODE", _processFilter);
        }

        #endregion
    }
}
#endif
