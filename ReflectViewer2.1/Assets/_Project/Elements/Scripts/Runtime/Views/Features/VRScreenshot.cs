using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VRScreenshot : MonoBehaviour
{
    public Canvas canvas;
    public Camera cam;
    public bool vrMode;
    public ScreenshotManager manager;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            manager.CreateScreenshot(canvas, true, vrMode, cam);
        }
    }

}
