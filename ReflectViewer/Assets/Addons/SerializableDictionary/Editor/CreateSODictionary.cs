#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SerializableDictionary
{
    public class CreateSODictionary : EditorWindow
    {
        #region VARIABLES

        private string _namespace   = String.Empty;
        private string _soClassName = String.Empty;
        private string _sClassName  = String.Empty;

        private PrimitiveType _keyType   = PrimitiveType.StringType;
        private string     _key       = String.Empty;
        private PrimitiveType _valueType = PrimitiveType.StringType;
        private string     _value     = String.Empty;

        private TextAsset _soDatabaseTemplate;
        private string    _data = String.Empty;

        #endregion

        #region WINDOW METHODS

        public static CreateSODictionary _window;

        [MenuItem("Dictionary/Create/SO-Dictionary", priority = 20)]
        public static void OpenWindow()
        {
            _window = GetWindow<CreateSODictionary>("Create SO Dictionary");
            _window.Show();
            _window.minSize = new Vector2(450, 200);
        }

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _soDatabaseTemplate = Resources.Load<TextAsset>("template_so_dictionary");

            if (EditorPrefs.HasKey("$NAMESPACE")) _namespace = EditorPrefs.GetString("$NAMESPACE");
        }

        public void OnGUI()
        {
            EditorGUILayout.ObjectField("Template:", _soDatabaseTemplate, typeof(TextAsset), false);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            _namespace = EditorGUILayout.TextField("Namespace:", _namespace);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString("$NAMESPACE", _namespace);
            }

            EditorGUILayout.Space();
            _soClassName = EditorGUILayout.TextField("SO Database Class Name:", _soClassName);
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
                if (string.IsNullOrEmpty(_soClassName) || string.IsNullOrEmpty(_sClassName))
                {
                    Debug.LogError("Cannot have Empty Class Name!");
                    return;
                }

                _data = _soDatabaseTemplate.text.Replace("%NAME", _soClassName);
                _data = _data.Replace("%NAMESPACE", _namespace);
                _data = _data.Replace("%S_DICTIONARY", _sClassName);
                _data = _data.Replace("%KEY",
                                      _keyType != PrimitiveType.CustomType
                                          ? MyType.TypesDictionary[_keyType]
                                          : _key);
                _data = _data.Replace("%VALUE",
                                      _valueType != PrimitiveType.CustomType
                                          ? MyType.TypesDictionary[_valueType]
                                          : _value);

                string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, _soClassName, "cs");
                File.WriteAllText(path, _data);
                AssetDatabase.Refresh();
            }

            #endregion
        }
    }
}
#endif
