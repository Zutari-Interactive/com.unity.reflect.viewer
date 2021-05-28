using System.Collections;
using System.Collections.Generic;
using Unity.Reflect.Viewer.UI;
using Unity.TouchFramework;
using UnityEngine;
using UnityEngine.Reflect;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public struct ViewSettings
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isOrthographic;
    public float viewSize;
}

public class StaticViewCollection : MonoBehaviour
{

    public List<Camera> viewCameras = new List<Camera>();

    public List<ViewSettings> views = new List<ViewSettings>();

    public float tweenTime;

    public AnimationCurve cameraMoveCurve;

    private Transform originalCamPos;

    private GameObject mainCam;

    private int activeIndex;

    private int currentlyActiveCameraIndex;

    private ViewsDialogController viewsController;

    private ViewCameraDialogController viewCameraSettingController;

    private void Start()
    {
        viewsController = FindObjectOfType<ViewsDialogController>();
        viewCameraSettingController = FindObjectOfType<ViewCameraDialogController>();
        mainCam = Camera.main.gameObject;
        MinMaxPropertyControl sliderValue = viewCameraSettingController.GetComponentInChildren<MinMaxPropertyControl>();
        sliderValue.onFloatValueChanged.AddListener(UpdateZoomValue);
    }

    //Deprecated
    public void AddCamera(Camera c)
    {
        viewCameras.Add(c);
        AddViewButton();
    }

    public void CreateNewView(Transform t, bool ortho)
    {
        ViewSettings v = new ViewSettings()
        {
            position = t.position,
            rotation = t.rotation,
            isOrthographic = ortho,
            viewSize = 40f              //default view size if ortho
        };

        views.Add(v);
        AddViewButton();

    }

    public void MoveCameraToView(int i)
    {
        FreeFlyCamera ffCam = mainCam.GetComponent<FreeFlyCamera>();
        ffCam.enabled = false;
        activeIndex = i;
        StopCoroutine(RunMove());
        StartCoroutine(RunMove());
    }

    IEnumerator RunMove()
    { 
        Tween.Position(mainCam.transform, views[activeIndex].position, tweenTime, 0.1f, cameraMoveCurve);
        Tween.Rotation(mainCam.transform, views[activeIndex].rotation, tweenTime, 0.1f, cameraMoveCurve);

        yield return new WaitForSecondsRealtime(tweenTime);

        if (views[activeIndex].isOrthographic)
        {
            Camera c = mainCam.GetComponent<Camera>();
            c.orthographic = true;
            c.orthographicSize = views[activeIndex].viewSize;
        }
    }

    public void LogCamPosition()
    {
        originalCamPos = mainCam.transform;
    }

    //Deprecated
    public void ActivateCameraView(int i)
    {
        if (mainCam.activeInHierarchy)
        {
            mainCam.SetActive(false);
        }
        viewCameras[i].enabled = true;
        currentlyActiveCameraIndex = i;
    }

    //Deprecated
    public void DeActivateCameraView()
    {
        viewCameras[currentlyActiveCameraIndex].enabled = false;
    }

    private void AddViewButton()
    {
        viewsController.CreateToolButton();
    }

    public void ReturnCameraToOriginalPosition()
    {
        Camera c = mainCam.GetComponent<Camera>();
        c.orthographic = false;

        StopCoroutine(RunMove());

        Debug.Log("return to original position");
        mainCam.transform.position = originalCamPos.position;
        mainCam.transform.rotation = originalCamPos.rotation;

        FreeFlyCamera ffCam = mainCam.GetComponent<FreeFlyCamera>();
        ffCam.enabled = true;
    }

    //Deprecated
    public void ReturnToMainCamera()
    {
        if (!mainCam.activeInHierarchy)
        {
            DeActivateCameraView();
            mainCam.SetActive(true);
        }
    }


    //Deprecated
    public Camera FetchActiveCamera()
    {
        return viewCameras[currentlyActiveCameraIndex];
    }

    public void UpdateZoomValue(float f)
    {
        if (viewCameras[currentlyActiveCameraIndex].orthographic)
        {
            viewCameras[currentlyActiveCameraIndex].orthographicSize = f;
        }
    }

}
