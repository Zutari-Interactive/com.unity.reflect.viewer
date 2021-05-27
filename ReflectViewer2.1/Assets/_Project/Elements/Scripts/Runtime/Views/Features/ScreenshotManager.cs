using Elements.General.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{

    private Canvas canvas;

    public void CreateScreenshot(Canvas c, bool hideUI)
    {
        canvas = c;
        if (hideUI)
        {
            
            StartCoroutine(SaveImage());
            Debug.Log("finished awaiting screenshot");
            
        }
        else
        {
            StartCoroutine(SaveImage());
        }

    }

    IEnumerator SaveImage()
    {
        string appPath = Application.dataPath;
        var savePath = FileBrowser.OpenFolderPanel(appPath);

        string actualPath = savePath[0];                                //this is very tenuous, need tightening up, currently assumes only one string is in the array

        canvas.enabled = false;

        ScreenCapture.CaptureScreenshot(SaveFileBrowser(actualPath));

        yield return new WaitForEndOfFrame();
        canvas.enabled = true;
        Debug.Log("taken screenshot");

        yield return null;
    }
    private string SaveFileBrowser(string savePath)
    {
       return FileBrowser.SaveFilePanel("Select Save Location", savePath, "untitled", "png");

    }
}
