using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;


public class StaticViewCollection : MonoBehaviour
{
    public List<Camera> viewCameras = new List<Camera>();

    private GameObject viewUI; 

    private int currentlyActiveCameraIndex;

    public void AddCamera(Camera c)
    {
        viewCameras.Add(c);
    }

    public void ActivateCameraView(int i)
    {
        viewCameras[i].enabled = true;
        currentlyActiveCameraIndex = i;
    }

    public void DeActivateCameraView()
    {
        viewCameras[currentlyActiveCameraIndex].enabled = false;
    }
}
