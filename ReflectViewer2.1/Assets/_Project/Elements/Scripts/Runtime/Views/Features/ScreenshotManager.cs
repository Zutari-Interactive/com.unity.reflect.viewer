using Elements.General.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{

    public void CreateScreenshot(Camera cam, bool hideUI)
    {
        

        if (hideUI)
        {
            int uiLayer = LayerMask.NameToLayer("UI");
            int oldMask = cam.cullingMask;
            cam.cullingMask = 1 >> uiLayer;

            SaveImage();

            cam.cullingMask = oldMask;
        }
        else
        {
            SaveImage();
        }

    }

    private void SaveImage()
    {
        string appPath = Application.dataPath;
        var savePath = FileBrowser.OpenFolderPanel(appPath);

        string actualPath = savePath[0];                                //this is very tenuous, need tightening up, currently assumes only one string is in the array

        ScreenCapture.CaptureScreenshot(SaveFileBrowser(actualPath));
    }
    private string SaveFileBrowser(string savePath)
    {
       return FileBrowser.SaveFilePanel("Select Save Location", savePath, "untitled", "png");

    }
}
