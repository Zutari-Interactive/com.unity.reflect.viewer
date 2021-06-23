using Elements.General.IO;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenshotManager : MonoBehaviour
{
    private Camera screenshotCam;
    private Canvas canvas;
    private bool vrMode;
    private int counter = 0;
    private bool grab;

    public void CreateScreenshot(Canvas c, bool hideUI, bool mode, Camera cam)
    {
        vrMode = mode;
        canvas = c;

        if(screenshotCam == null)
        {
            Debug.Log("assign camera");
            screenshotCam = cam;
        }

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
            RenderPipelineManager.endCameraRendering += OnEndCamerRenedering;
            grab = true;
            //yield return new WaitForEndOfFrame();
            //VRScreenshot();
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

    private void OnEndCamerRenedering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (grab)
        {
            Debug.Log("using Camera = " + arg2.name);
            VRScreenshot();
        }
    }

    private string SaveFileBrowser(string savePath)
    {
       return FileBrowser.SaveFilePanel("Select Save Location", savePath, "untitled", "png");
    }

    private void VRScreenshot()
    {
        Debug.Log("starting VR screenshot");
        counter++;

        var filename = "VR_screenshot_0" + counter.ToString() + ".png";
        var path = Application.dataPath + "/" + filename;
        Debug.Log(path);

        RenderTexture rt = screenshotCam.targetTexture;

        Texture2D renderResult = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, rt.width, rt.height);
        renderResult.ReadPixels(rect, 0, 0);
        byte[] bytes = renderResult.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        grab = false;
        Debug.Log("VR screenshot complete");
    }

    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= OnEndCamerRenedering;
    }

}
