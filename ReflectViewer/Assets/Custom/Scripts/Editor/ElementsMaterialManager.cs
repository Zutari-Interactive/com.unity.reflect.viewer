using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Elements.Database;

public class ElementsMaterialManager : EditorWindow
{
    [MenuItem("Window/UIElements/ElementsMaterialManager")]
    public static void ShowExample()
    {
        ElementsMaterialManager wnd = GetWindow<ElementsMaterialManager>();
        wnd.titleContent = new GUIContent("ElementsMaterialManager");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        // VisualElement label = new Label("Elements Material Manager");
        //root.Add(label);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Custom/Scripts/Editor/ElementsMaterialManager.uss");
        VisualElement labelWithStyle = new Label("Elements Material Manager");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Custom/Scripts/Editor/ElementsMaterialManager.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        // Get a reference to the field from UXML and assign it its value.
        var uxmlField = root.Q<ObjectField>("mat-lib-field");
        uxmlField.value = new ReplaceMaterialLibrary { name = "material library" };

        // Create a new field, disable it, and give it a style class.
        var csharpField = new ObjectField("Material Library");
        csharpField.SetEnabled(false);
        csharpField.AddToClassList("some-styled-field");
        csharpField.value = uxmlField.value;
        root.Add(csharpField);

        // Mirror value of uxml field into the C# field.
        uxmlField.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
        {

            csharpField.value = evt.newValue;
        });

        // Action to perform when button is pressed.
        // Toggles the text on all buttons in 'container'.
        Action action = () =>
        {
            root.Query<Button>().ForEach((button) =>
            {
                Debug.Log("button press");
                button.text = button.text.EndsWith("Button") ? "Materials updated" : "Update Materials";
            });
        };

        // Get a reference to the Button from UXML and assign it its action.
        var uxmlButton = root.Q<Button>("update-mat-button");
        uxmlButton.RegisterCallback<MouseUpEvent>((evt) => action());

        // Create a new Button with an action and give it a style class.
        var csharpButton = new Button(action) { text = "Update Materials" };
        csharpButton.AddToClassList("update-mat-button");
        root.Add(csharpButton);


    }
}
