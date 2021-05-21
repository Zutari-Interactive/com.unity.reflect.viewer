using System.Collections;
using System.Collections.Generic;
using Unity.Reflect.Viewer.UI;
using UnityEngine;
using UnityEngine.Reflect;


public class StaticViewCollection : MonoBehaviour
{
    public List<Camera> viewCameras = new List<Camera>();

    private GameObject mainCam; 

    private int currentlyActiveCameraIndex;

    private ViewsDialogController viewsController;

    private void Start()
    {
        viewsController = FindObjectOfType<ViewsDialogController>();
        mainCam = Camera.main.gameObject;
    }

    public void AddCamera(Camera c)
    {
        viewCameras.Add(c);
        AddViewButton();
    }

    public void ActivateCameraView(int i)
    {
        if (mainCam.activeInHierarchy)
        {
            mainCam.SetActive(false);
        }
        viewCameras[i].enabled = true;
        currentlyActiveCameraIndex = i;
    }

    public void DeActivateCameraView()
    {
        viewCameras[currentlyActiveCameraIndex].enabled = false;
    }

    private void AddViewButton()
    {
        viewsController.CreateToolButton();
    }

    public void ReturnToMainCamera()
    {
        if (!mainCam.activeInHierarchy)
        {
            DeActivateCameraView();
            mainCam.SetActive(true);
        }
    }
}
