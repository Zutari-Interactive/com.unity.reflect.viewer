using Elements.General.IO;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenshotManager : MonoBehaviour
{
    public GameObject confirmText;
    private Camera screenshotCam;
    private Canvas canvas;
    private bool vrMode;
    private int counter = 0;
    private bool grab;

    private void OnEnable()
    {
        confirmText.SetActive(false);
    }

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
            RenderPipelineManager.endCameraRendering += OnEndCamerRendering;
            grab = true;
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

    private void OnEndCamerRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (grab)
        {
            VRScreenshot();
        }
    }

    private string SaveFileBrowser(string savePath)
    {
       return FileBrowser.SaveFilePanel("Select Save Location", savePath, "untitled", "png");
    }

    private void VRScreenshot()
    {
        counter++;
        Debug.Log("Taking VR Screenshot");
        var filename = "VR_screenshot_0" + counter.ToString() + ".png";
        var path = Application.dataPath + "/" + filename;

        RenderTexture crt = RenderTexture.active;
        Texture2D renderResult = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

        RenderTexture rt = screenshotCam.targetTexture;
        RenderTexture.active = rt;
        
        Rect rect = new Rect(0, 0, 1920, 1080);
        renderResult.ReadPixels(rect, 0, 0);

        //TODO - move this to another thread?
        Color[] pixels = renderResult.GetPixels();
        for (int p = 0; p < pixels.Length; p++)
        {
            pixels[p] = pixels[p].gamma;
        }
        renderResult.SetPixels(pixels);

        renderResult.Apply();

        byte[] bytes = renderResult.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        RenderTexture.active = crt;
        grab = false;
        Debug.Log("VR screenshot complete");
        confirmText.SetActive(true);
        Invoke("NotificationTurnOff", 1f);
    }

    private void NotificationTurnOff()
    {
        confirmText.SetActive(false);
    }

    //IEnumerator ChangePixelsToGamma()
    //{

    //}

    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= OnEndCamerRendering;
    }

}
