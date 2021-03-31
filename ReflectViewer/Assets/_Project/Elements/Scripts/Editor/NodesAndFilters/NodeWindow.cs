#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Zutari.Elements.Editors
{
    public enum NodeType
    {
        UniqueNode,
        UniversalNode,
    }

    public class NodeWindow : EditorWindow
    {
        #region WINDOW

        public static NodeWindow Window;

        [MenuItem("Elements/Create/Node")]
        public static void OpenWindow()
        {
            Window = GetWindow<NodeWindow>("Node Creator");
            Window.Show();
        }

        #endregion

        #region VARIABLES

        private string _namespace       = string.Empty;
        private string _nodeClassName   = string.Empty;
        private string _filterClassName = string.Empty;
        private string _categoryName    = string.Empty;
        private string _subcategoryName = string.Empty;

        private TextAsset _nodeTemplate;
        private NodeType  _nodeType = NodeType.UniqueNode;

        #endregion

        #region UNITY METHODS

        public void OnEnable()
        {
            _nodeTemplate = Resources.Load<TextAsset>("template-unique-node");
            if (EditorPrefs.HasKey("$NAMESPACE")) _namespace = EditorPrefs.GetString("$NAMESPACE");
        }

        public void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            _nodeType = (NodeType) EditorGUILayout.EnumPopup("Node Type", _nodeType);
            EditorGUILayout.Space();

            if (EditorGUI.EndChangeCheck())
            {
                switch (_nodeType)
                {
                    case NodeType.UniqueNode:
                        _nodeTemplate = Resources.Load<TextAsset>("template-unique-node");
                        break;
                    case NodeType.UniversalNode:
                        _nodeTemplate = Resources.Load<TextAsset>("template-universal-node");
                        break;
                }
            }

            _nodeTemplate =
                (TextAsset) EditorGUILayout.ObjectField("Template:", _nodeTemplate, typeof(TextAsset), false);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            _namespace = EditorGUILayout.TextField("Namespace:", _namespace);

            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString("$NAMESPACE", _namespace);
            }

            _nodeClassName = EditorGUILayout.TextField("Node Class Name:", _nodeClassName);

            _filterClassName = EditorGUILayout.TextField("Filter Class Name:", _filterClassName);
            EditorGUILayout.Space();

            _categoryName = EditorGUILayout.TextField("Category Name:", _categoryName);

            if (_nodeType == NodeType.UniqueNode)
            {
                _subcategoryName = EditorGUILayout.TextField("SubCategory Name:", _subcategoryName);
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Create Custom Node"))
            {
                if (string.IsNullOrEmpty(_nodeClassName) || string.IsNullOrEmpty(_filterClassName))
                {
                    Debug.LogError("Cannot have Empty Class Name!");
                    return;
                }

                string data = string.Empty;
                switch (_nodeType)
                {
                    case NodeType.UniqueNode:
                        UpdateTextForUniqueNode(out data);
                        break;
                    case NodeType.UniversalNode:
                        UpdateTextForUniversalNode(out data);
                        break;
                }

                string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, _nodeClassName, "cs");
                File.WriteAllText(path, data);
                AssetDatabase.Refresh();
            }
        }

        #endregion

        #region METHODS

        private void UpdateTextForUniqueNode(out string data)
        {
            data = _nodeTemplate.text.Replace("$NAMESPACE", _namespace);
            data = data.Replace("$UNIQUENODENAME", _nodeClassName);
            data = data.Replace("$NODEPROCESSORFILTERNAME", _filterClassName);
            data = data.Replace("$CATEGORY", _categoryName);
            data = data.Replace("$SUBCATEGORY", _subcategoryName);
        }

        private void UpdateTextForUniversalNode(out string data)
        {
            data = _nodeTemplate.text.Replace("$NAMESPACE", _namespace);
            data = data.Replace("$UNIVERSALNODENAME", _nodeClassName);
            data = data.Replace("$NODEPROCESSORFILTERNAME", _filterClassName);
            data = data.Replace("$CATEGORY", _categoryName);
        }

        #endregion
    }
}
#endif
