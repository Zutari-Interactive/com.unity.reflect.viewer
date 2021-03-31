#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SerializableDictionary
{
    public class CreateCDictionary : EditorWindow
    {
        #region VARIABLES

        private string _namespace  = String.Empty;
        private string _cClassName = String.Empty;
        private string _sClassName = String.Empty;

        private PrimitiveType _keyType   = PrimitiveType.StringType;
        private string        _key       = String.Empty;
        private PrimitiveType _valueType = PrimitiveType.StringType;
        private string        _value     = String.Empty;

        private TextAsset _cDatabaseTemplate;
        private string    _data = String.Empty;

        #endregion

        #region WINDOW METHODS

        public static CreateCDictionary _window;

        [MenuItem("Dictionary/Create/C-Dictionary", priority = 20)]
        public static void OpenWindow()
        {
            _window = GetWindow<CreateCDictionary>("Create C Dictionary");
            _window.Show();
            _window.minSize = new Vector2(450, 200);
        }

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _cDatabaseTemplate = Resources.Load<TextAsset>("template_c_dictionary");
            if (EditorPrefs.HasKey("$NAMESPACE")) _namespace = EditorPrefs.GetString("$NAMESPACE");
        }

        public void OnGUI()
        {
            _cDatabaseTemplate =
                (TextAsset) EditorGUILayout.ObjectField("Template:", _cDatabaseTemplate, typeof(TextAsset), false);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            _namespace = EditorGUILayout.TextField("Namespace:", _namespace);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString("$NAMESPACE", _namespace);
            }

            EditorGUILayout.Space();
            _cClassName = EditorGUILayout.TextField("C Database Class Name:", _cClassName);
            _sClassName = EditorGUILayout.TextField("S Dictionary Class Name:", _sClassName);

            EditorGUILayout.Space();

            _keyType = (PrimitiveType) EditorGUILayout.EnumPopup("Select Key Type", _keyType);
            _valueType = (PrimitiveType) EditorGUILayout.EnumPopup("Select Value Type", _valueType);

            if (_keyType == PrimitiveType.CustomType)
            {
                _key = EditorGUILayout.TextField("Custom Key Type", _key);
            }

            if (_valueType == PrimitiveType.CustomType)
            {
                _value = EditorGUILayout.TextField("Custom Value Type", _value);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Create New SODatabase"))
            {
                if (string.IsNullOrEmpty(_cClassName) || string.IsNullOrEmpty(_sClassName))
                {
                    Debug.LogError("Cannot have Empty Class Name!");
                    return;
                }

                _data = _cDatabaseTemplate.text.Replace("%C_DATABASE", _cClassName);
                _data = _data.Replace("%NAMESPACE", _namespace);
                _data = _data.Replace("%S_DICTIONARY", _sClassName);
                _data = _data.Replace("%KEY",
                                      _keyType != PrimitiveType.CustomType ? MyType.TypesDictionary[_keyType] : _key);
                _data = _data.Replace("%VALUE",
                                      _valueType != PrimitiveType.CustomType
                                          ? MyType.TypesDictionary[_valueType]
                                          : _value);

                string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, _cClassName, "cs");
                File.WriteAllText(path, _data);
                AssetDatabase.Refresh();
            }

            #endregion
        }
    }
}
#endif
