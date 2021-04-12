#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Variables
{
    [CustomEditor(typeof(VariablesBase), true)]
    public class VariablesEditor : Editor
    {
        #region VARIABLES

        private VariablesBase _target;
        private string        _filename = string.Empty;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _target = target as VariablesBase;
            if (string.IsNullOrEmpty(_target.Filename)) _target.Filename = $"{_target.name}.json";
        }

        public override void OnInspectorGUI()
        {
            ButtonFields();
            base.OnInspectorGUI();
        }

        #endregion

        #region METHODS

        private void ButtonFields()
        {
            using (EditorGUILayout.VerticalScope verticalScope =
                new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                AssignSelf();
                Persistence();
                FilenameField();
            }

            EditorGUILayout.Space();
        }

        private void AssignSelf()
        {
            if (GUILayout.Button("Assign", EditorStyles.miniButton))
            {
                List<GameObject> rootObjects = new List<GameObject>();
                int sceneCount = EditorSceneManager.sceneCount;

                for (int i = 0; i < sceneCount; i++)
                {
                    EditorSceneManager.GetSceneAt(i).GetRootGameObjects(rootObjects);
                    foreach (GameObject rootObject in rootObjects)
                    {
                        foreach (MonoBehaviour behaviour in rootObject.GetComponentsInChildren<MonoBehaviour>(true))
                        {
                            foreach (FieldInfo variable in behaviour.GetType().GetFields())
                            {
                                if (variable.FieldType.IsInstanceOfType(_target))
                                {
                                    variable.SetValue(behaviour, _target);
                                }
                            }
                        }
                    }
                }

                EditorSceneManager.MarkAllScenesDirty();
                EditorUtility.SetDirty(this);
            }
        }

        private void Persistence()
        {
            using (EditorGUILayout.HorizontalScope hScope = new EditorGUILayout.HorizontalScope())
            {
                Serialize();
                Deserialize();
                Delete();
                ShowFolder();
            }

            EditorGUILayout.Space();
        }

        private void FilenameField()
        {
            using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                _target.Filename = EditorGUILayout.TextField("Filename:", _target.Filename);
                if (changeCheckScope.changed)
                {
                    if (!string.IsNullOrEmpty(_target.Filename) && !_target.Filename.Contains(".json"))
                        _target.Filename = $"{_target.Filename}.json";
                    EditorUtility.SetDirty(_target);
                }
            }
        }

        private void Serialize()
        {
            if (!GUILayout.Button("Save", EditorStyles.miniButtonLeft)) return;
            _target.WriteToFile();
            AssetDatabase.Refresh();
        }

        private void Deserialize()
        {
            if (!GUILayout.Button("Load", EditorStyles.miniButtonMid)) return;
            _target.OverwriteFromFile();
            EditorUtility.SetDirty(_target);
        }

        private void Delete()
        {
            if (!GUILayout.Button("Delete", EditorStyles.miniButtonMid)) return;
            _target.DeleteFile();
            AssetDatabase.Refresh();
        }

        private void ShowFolder()
        {
            if (!GUILayout.Button("Show Folder", EditorStyles.miniButtonRight)) return;
            EditorUtility.RevealInFinder(_target.FilePath);
        }

        #endregion
    }
}
#endif
