
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(EditorMaterialFinder))]
public class MaterialManagerEditor : Editor
{
    Object revitLibrary;

    // [MenuItem("Zutari/Material Manager")]
    public static void ShowWindow()
    {
        //EditorWindow.GetWindow<MaterialManagerEditor>("Zutari Material Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Revit Material Library", EditorStyles.boldLabel);

        //revitLibrary = EditorGUILayout.ObjectField(revitLibrary, typeof(MaterialLibrary), true);

        GUILayout.Label("Substance Material Library", EditorStyles.boldLabel);

        GUILayout.Label("Custom Material Library", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Swap Materials"))
        {
            Debug.Log("swap materials");

        }

        //Don't allow this until materials have been swapped
        if (GUILayout.Button("Delete Revit Materials"))
        {

            Debug.Log("Delete Revit Materials");
        }

        EditorGUILayout.EndHorizontal();
    }


}
