#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Variables
{
    public class VariablesWindow : EditorWindow
    {
        #region WINDOW

        public static VariablesWindow Window;

        [MenuItem("Window/Variables/Create Variables Script")]
        [MenuItem("Assets/Create/Variables/New Variables Script", false, 90)]
        public static void OpenWindow()
        {
            Window = GetWindow<VariablesWindow>("Variables Script Creator");
            Window.Show();
            Window.minSize = new Vector2(320, 96);
            Window.maxSize = new Vector2(512, 96);
        }

        #endregion

        #region VARIABLES

        private string _scriptName = string.Empty;
        private string _namespace = string.Empty;
        private TextAsset _template;

        private static string SCRIPTNAME_KEY = "$SCRIPTNAME";
        private static string NAMESPACE_KEY = "$NAMESPACE";
        private static string PATH_KEY = "$PATH";

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _template = Resources.Load<TextAsset>("variables-template");

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
            _template = EditorGUILayout.ObjectField("Template:", _template, typeof(TextAsset), false) as TextAsset;
        }

        private void NamespaceField()
        {
            using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                _namespace = EditorGUILayout.TextField("Namespace:", _namespace);
                if (!changeCheckScope.changed) return;
                if (string.IsNullOrEmpty(_namespace)) return;
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
            if (!_template || string.IsNullOrEmpty(_namespace) || string.IsNullOrEmpty(_scriptName)) return;

            string text = _template.text;
            text = text.Replace(SCRIPTNAME_KEY, _scriptName);
            text = text.Replace(NAMESPACE_KEY, _namespace);

            string path = EditorUtility.SaveFilePanelInProject("Save Script", $"{_scriptName}", "cs", "");
            if (string.IsNullOrEmpty(path)) return;

            File.WriteAllText(path, text);

            EditorPrefs.SetString(SCRIPTNAME_KEY, _scriptName);
            EditorPrefs.SetString(NAMESPACE_KEY, _namespace);
            EditorPrefs.SetString(PATH_KEY, Path.GetDirectoryName(path));

            AssetDatabase.Refresh();
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
