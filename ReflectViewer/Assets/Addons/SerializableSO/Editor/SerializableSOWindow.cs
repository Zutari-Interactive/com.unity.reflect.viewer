#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SerializableSO
{
    public enum SerializableTemplate
    {
        JsonUtilityTemplate,
        JsonNetTemplate,
    }

    public class SerializableSOWindow : EditorWindow
    {
        #region WINDOW

        public static SerializableSOWindow Window;

        [MenuItem("Assets/Create/C# Templates/Serializable-SO")]
        public static void OpenWindow()
        {
            Window = GetWindow<SerializableSOWindow>("SerializableSO Creator");
            Window.Show();
            Window.minSize = new Vector2(320, 128);
            Window.maxSize = new Vector2(512, 128);

            Object selectedObject = Selection.activeObject;
            _path = AssetDatabase.GetAssetPath(selectedObject);
            if (string.IsNullOrEmpty(_path)) return;
            if (!Directory.Exists(_path)) _path = Path.GetDirectoryName(_path);
        }

        #endregion

        #region VARIABLES

        private string _scriptName = string.Empty;
        private string _namespace = "SerializableSO";
        private static string _path = string.Empty;
        private TextAsset _template;
        private SerializableTemplate _serializableTemplate = SerializableTemplate.JsonUtilityTemplate;

        private static string SCRIPTNAME_KEY = "$SCRIPTNAME";
        private static string NAMESPACE_KEY = "$NAMESPACE";
        private static string PATH_KEY = "$PATH";

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            EnumToTemplate();

            if (EditorPrefs.HasKey(NAMESPACE_KEY))
                _namespace = EditorPrefs.GetString(NAMESPACE_KEY);
        }

        public void OnGUI()
        {
            TemplateField();
            NamespaceField();
            ScriptNameField();
            CreateScriptButton();
        }

        #endregion

        #region METHODS

        private void TemplateField()
        {
            using (EditorGUI.ChangeCheckScope checkScope = new EditorGUI.ChangeCheckScope())
            {
                _serializableTemplate =
                    (SerializableTemplate) EditorGUILayout.EnumPopup("Template:", _serializableTemplate);

                if (checkScope.changed)
                {
                    EnumToTemplate();
                }
            }

            _template = EditorGUILayout.ObjectField("Template:", _template, typeof(TextAsset), false) as TextAsset;
            EditorGUILayout.Space();
        }

        private void NamespaceField()
        {
            using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                _namespace = EditorGUILayout.TextField("Namespace:", _namespace);
                if (!changeCheckScope.changed) return;
                if (string.IsNullOrEmpty(_namespace)) _namespace = "SerializableSO";
                EditorPrefs.SetString(NAMESPACE_KEY, _namespace);
            }
        }

        private void ScriptNameField()
        {
            _scriptName = EditorGUILayout.TextField("Script Name:", _scriptName);
        }

        private void CreateScriptButton()
        {
            if (!GUILayout.Button("Create Script")) return;
            if (!_template || string.IsNullOrEmpty(_scriptName)) return;

            string text = _template.text;
            if (string.IsNullOrEmpty(_namespace)) _namespace = "SerializableSO";
            text = text.Replace(NAMESPACE_KEY, _namespace);
            text = text.Replace(SCRIPTNAME_KEY, _scriptName);

            if (string.IsNullOrEmpty(_path))
                _path = EditorUtility.SaveFilePanelInProject("Save Script", $"{_scriptName}", "cs", "");
            else _path = Path.Combine(_path, $"{_scriptName}.cs");
            if (string.IsNullOrEmpty(_path)) return;

            File.WriteAllText(_path, text);

            EditorPrefs.SetString(SCRIPTNAME_KEY, _scriptName);
            EditorPrefs.SetString(NAMESPACE_KEY, _namespace);
            EditorPrefs.SetString(PATH_KEY, Path.GetDirectoryName(_path));

            AssetDatabase.Refresh();
        }

        private void EnumToTemplate()
        {
            switch (_serializableTemplate)
            {
                case SerializableTemplate.JsonUtilityTemplate:
                    _template = Resources.Load<TextAsset>("template-json.utility");
                    break;
                case SerializableTemplate.JsonNetTemplate:
                    _template = Resources.Load<TextAsset>("template-json.net");
                    break;
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CreateScriptableObject()
        {
            string path = EditorPrefs.GetString(PATH_KEY, string.Empty);
            string scriptName = EditorPrefs.GetString(SCRIPTNAME_KEY, string.Empty);
            string nameSpace = EditorPrefs.GetString(NAMESPACE_KEY, string.Empty);

            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(scriptName) ||
                string.IsNullOrEmpty(nameSpace)) return;

            EditorPrefs.DeleteKey(PATH_KEY);
            EditorPrefs.DeleteKey(SCRIPTNAME_KEY);
            EditorPrefs.DeleteKey(NAMESPACE_KEY);

            ScriptableObject asset = CreateInstance($"{nameSpace}.{scriptName}");
            AssetDatabase.CreateAsset(asset, Path.Combine(path, $"{scriptName}.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
#endif
