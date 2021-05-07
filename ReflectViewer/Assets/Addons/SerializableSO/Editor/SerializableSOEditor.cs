#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SerializableSO
{
    [CustomEditor(typeof(SerializableSOBase), true)]
    public class SerializableSOEditor : Editor
    {
        #region VARIABLES

        private SerializableSOBase _target;
        private bool _showPath = false;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _target = target as SerializableSOBase;
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
                SerializablePathField();
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
                EditorUtility.SetDirty(_target);
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

        private void SerializablePathField()
        {
            using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                _showPath = EditorGUILayout.ToggleLeft("", _showPath);
                _target.SerializablePath =
                    (SerializablePath) EditorGUILayout.EnumPopup("Serializable Path:", _target.SerializablePath);

                if (_showPath)
                {
                    EditorGUILayout.TextField(_target.GetFilePath(), EditorStyles.wordWrappedLabel, GUILayout.Height(32f));
                    EditorGUILayout.Space();
                }

                if (changeCheckScope.changed)
                {
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
            EditorUtility.RevealInFinder(_target.GetFilePath());
        }

        #endregion
    }
}
#endif