using Elements.General.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{

    private Canvas canvas;
    private bool vrMode;

    public void CreateScreenshot(Canvas c, bool hideUI, bool mode)
    {
        vrMode = mode;
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
        if (vrMode)
        {
            ScreenCapture.CaptureScreenshot(Application.dataPath);          //this path will put the files in the executablename/Data folder
        }
        else
        {
            string appPath = Application.dataPath;
            var savePath = FileBrowser.OpenFolderPanel(appPath);

            string actualPath = savePath[0];                                //this is very tenuous, need tightening up, currently assumes only one string is in the array

            canvas.enabled = false;

            ScreenCapture.CaptureScreenshot(SaveFileBrowser(actualPath));

            yield return new WaitForEndOfFrame();
            canvas.enabled = true;
            Debug.Log("taken screenshot");
        }

        yield return null;
    }
    private string SaveFileBrowser(string savePath)
    {
       return FileBrowser.SaveFilePanel("Select Save Location", savePath, "untitled", "png");

    }
}
