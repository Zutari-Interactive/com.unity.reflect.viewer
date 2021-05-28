using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Reflect;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ViewManager : CustomNode
{

    private Vector3 viewPos;
    private Quaternion viewRot;

    private GameObject viewPrefab;
    private StaticViewCollection viewCollection;

    private POI poi;

    private bool isOrthographic;
    private bool viewsPresent;


    void OnEnable()
    {
        Addressables.LoadAssetAsync<GameObject>("View").Completed += OnAddressableLoadDone;
        viewCollection = GetComponent<StaticViewCollection>();
    }

    public void SetupView(Metadata data)
    {
        poi = FetchPOIComponent(data.gameObject);
        if (poi == null)
        {
            Debug.LogError("no POI found on this view, aborting view creation");
            return;
        }
        isOrthographic = poi.orthographic;
        viewCollection.CreateNewView(data.transform, isOrthographic);
        Debug.Log("created new view");
    }

    //deprecated
    public void CreateView(Metadata data)
    {
        if (!viewsPresent)
        {
            viewsPresent = true;
        }

        poi = FetchPOIComponent(data.gameObject);
        if(poi == null)
        {
            Debug.LogError("no POI found on this view, aborting view creation");
            return;
        }
        viewPos = data.gameObject.transform.position;
        viewRot = data.gameObject.transform.rotation;

        SetupView();

        Debug.Log("View created");
    }

    private POI FetchPOIComponent(GameObject obj)
    {
        return obj.GetComponent<POI>();
    }

    //deprecated
    private void SetupView()
    {
        GameObject newView = Instantiate(viewPrefab);

        Camera cam = newView.GetComponent<Camera>();
        cam.enabled = false;

        newView.transform.position = viewPos;
        newView.transform.rotation = viewRot;

        isOrthographic = poi.orthographic;

        if (isOrthographic)
        {
            cam.orthographic = isOrthographic;
        }

        viewCollection.AddCamera(cam);
    }

    private void OnAddressableLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Result != null)
        {
            viewPrefab = obj.Result;
        }
        else
        {
            Debug.LogWarning("No addressable asset found for View");
        }
    }
}
