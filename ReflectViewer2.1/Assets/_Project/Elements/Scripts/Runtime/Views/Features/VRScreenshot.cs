using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VRScreenshot : RenderPipeline
{
    public VRScreenshot(Camera cam)
    {
        ScriptableRenderContext context = new ScriptableRenderContext();
        Camera[] cams = new Camera[]
        {
            cam
        };
        Render(context, cams);
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var c in cameras)
        {
            EndCameraRendering(context, c);
        }
    }
}
