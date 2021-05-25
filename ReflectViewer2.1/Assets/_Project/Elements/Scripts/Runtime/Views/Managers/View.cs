using System.Collections;
using System.Collections.Generic;
using Unity.Reflect.Viewer.UI;
using UnityEngine;

public class View : MonoBehaviour
{
    public int index;
    private ViewCameraDialogController viewController;

    public void CreateView(ToolButton button)
    {
        viewController = FindObjectOfType<ViewCameraDialogController>();
        if(viewController == null)
        {
            Debug.Log("no view camera dialog controller found");
        }
        else
        {
            viewController.AddNewButton(button, index);
        }
    }
}
